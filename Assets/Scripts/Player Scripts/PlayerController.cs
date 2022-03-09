using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public MasterDictionary myMasterDictionary;
    public static PlayerController _Control;
    public static PlayerController GetPlayerController { get { return _Control; } }

    public bool usingMouse = true;

    public Weapon mainWeapon;
    public Weapon altWeapon;
    //public Weapon altWeapon; // "set Weapon" script
    [Space(10)]

    /*
    public RangedWeapon mainWeapon;
    public MeleeWeapon meleeWeap1;
    public SpecialWeapon specWeap1;
    public RangedWeapon altWeapon;
    public MeleeWeapon meleeWeap2;
    public SpecialWeapon specWeap2;
    */

    private PlayerInputActions playerInputActions;
    private UI_Manager uiControl;
    private SeraphController seraphControl;

    Vector2 direction;
    Vector2 rayOrigin;
    public LineRenderer AimLine;
    public LineRenderer RangeCircleMain;
    public LineRenderer RangeCircleAlt;
    public bool circleEnabled;
    private float ThetaScale = 0.01f;
    private int circleSize;
    private float Theta;

    [Space(20)]
    Rifle rifle;
    Repeater repeater;
    Blunderbuss blunderbuss;
    null_ranged pistol;

    Knife knife;
    Saber saber;
    Hammer hammer;
    Bat bat;

    sp_Mortar mortar;
    sp_Mark mark;

    null_ranged nullRanged;
    null_melee nullMelee;
    null_special nullSpec;


    void Awake()
    {
        _Control = this;

        rifle = FindObjectOfType<Rifle>();
        repeater = FindObjectOfType<Repeater>();
        blunderbuss = FindObjectOfType<Blunderbuss>();
        knife = FindObjectOfType<Knife>();
        saber = FindObjectOfType<Saber>();
        hammer = FindObjectOfType<Hammer>();
        bat = FindObjectOfType<Bat>();
        mortar = FindObjectOfType<sp_Mortar>();
        mark = FindObjectOfType<sp_Mark>();

        nullRanged = FindObjectOfType<null_ranged>();
        nullMelee = FindObjectOfType<null_melee>();
        nullSpec = FindObjectOfType<null_special>();
        

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    void Start()
    {
        seraphControl = SeraphController.GetSeraphController;
        uiControl = GameObject.Find("*** UI Manager").GetComponent<UI_Manager>();
        //UpdateWeapon();

        StartCoroutine(OnStart_UpdateSeraphs());
    }

    // Update is called once per frame
    void Update()
    {
        direction = UpdateDirection(usingMouse);
        rayOrigin = UpdateRayOrigin();
        UpdateAimLine(true, direction);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            
        }
    }

    public void FireWeapon(Weapon weap)
    {
        //print("firing");
        if (weap != null)
        {
            weap.Fire(rayOrigin, direction);
        }
    }

    public void ReloadBothWeapons()
    {
        if (mainWeapon.GetWeaponType() == WeaponManager.WeaponType.Ranged)
            mainWeapon.Reload();
        if (altWeapon.GetWeaponType() == WeaponManager.WeaponType.Ranged)
            altWeapon.Reload();
    }

    private Vector2 UpdateDirection(bool mouse)
    {
        if (mouse)
        {
            // Mouse Look Controls
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z * -1)); // invert cam Z to make 0
            return (targetPos - this.transform.position).normalized;
        }
        else
        {
            // Controller/Non Mouse Controls
            Vector2 aimVector = playerInputActions.Player.Aim.ReadValue<Vector2>();
            Vector2 moveVector = playerInputActions.Player.Move.ReadValue<Vector2>();

            if (aimVector.Equals(Vector2.zero))
            {
                if (moveVector.Equals(Vector2.zero))
                    aimVector = Vector2.up;
                else
                    aimVector = moveVector;
            }

            return aimVector;
        }
    }

    private Vector2 UpdateRayOrigin()
    {
        return new Vector3(this.transform.position.x,
        this.transform.position.y + 0.25f,
        this.transform.position.z);
    }

    public Vector2 GetOrigin() { return rayOrigin; }
    public Vector2 GetDirection() { return direction; }

    void UpdateAimLine(bool enabled, Vector2 dir)
    {
        float indicatorRange;
        float segmentRange;
        if (enabled)
        {
            if (!AimLine.enabled)
                AimLine.enabled = true;

            
            AimLine.SetPosition(0, rayOrigin);

            if (mainWeapon.GetWeaponType() == WeaponManager.WeaponType.Ranged && mainWeapon.isValidWeapon)
            {
                if (altWeapon.GetWeaponType() == WeaponManager.WeaponType.Ranged && altWeapon.isValidWeapon)
                {
                    if (mainWeapon.GetComponent<RangedWeapon>().range > altWeapon.GetComponent<RangedWeapon>().range)
                    {
                        indicatorRange = mainWeapon.GetComponent<RangedWeapon>().range;
                        segmentRange = altWeapon.GetComponent<RangedWeapon>().range;
                    }
                    else
                    {
                        indicatorRange = altWeapon.GetComponent<RangedWeapon>().range;
                        segmentRange = mainWeapon.GetComponent<RangedWeapon>().range;
                    }
                }
                else
                {
                    indicatorRange = mainWeapon.GetComponent<RangedWeapon>().range;
                    segmentRange = mainWeapon.GetComponent<RangedWeapon>().range;
                }
            }

            else if (altWeapon.isValidWeapon)
            {
                indicatorRange = altWeapon.GetComponent<RangedWeapon>().range;
                segmentRange = altWeapon.GetComponent<RangedWeapon>().range;
            }
            else
            {
                indicatorRange = 2f;
                segmentRange = 2f;
            }

            AimLine.SetPosition(1, rayOrigin + (dir * segmentRange));
            AimLine.SetPosition(2, rayOrigin + (dir * indicatorRange));
           
        }
        else
        {
            if (AimLine.enabled)
                AimLine.enabled = false;
        }
    }

    private void UpdateCircleMain(float range)
    {
        if (circleEnabled)
        {
            if (!RangeCircleMain.enabled)
                RangeCircleMain.enabled = true;

            Theta = 0f;
            circleSize = (int)((1f / ThetaScale) + 1f);
            RangeCircleMain.positionCount = circleSize;
            for (int i = 0; i < circleSize; i++)
            {
                Theta += (2.0f * Mathf.PI * ThetaScale);
                float x = rayOrigin.x + range * Mathf.Cos(Theta);
                float y = rayOrigin.y + range * Mathf.Sin(Theta);
                RangeCircleMain.SetPosition(i, new Vector3(x, y, 0));
            }
        }
        else
        {
            if (RangeCircleMain.enabled)
                RangeCircleMain.enabled = false;
        }
    }
    private void UpdateCircleAlt(float range)
    {
        if (circleEnabled)
        {
            if (!RangeCircleAlt.enabled)
                RangeCircleAlt.enabled = true;

            Theta = 0f;
            circleSize = (int)((1f / ThetaScale) + 1f);
            RangeCircleAlt.positionCount = circleSize;
            for (int i = 0; i < circleSize; i++)
            {
                Theta += (2.0f * Mathf.PI * ThetaScale);
                float x = rayOrigin.x + range * Mathf.Cos(Theta);
                float y = rayOrigin.y + range * Mathf.Sin(Theta);
                RangeCircleAlt.SetPosition(i, new Vector3(x, y, 0));
            }
        }
        else
        {
            if (RangeCircleAlt.enabled)
                RangeCircleAlt.enabled = false;
        }
    }

    public void ToggleAimLineColor(bool isRed)
    {
        if (isRed)
        {
            AimLine.startColor = Color.red;
            AimLine.endColor = Color.red;
        }
        else
        {
            AimLine.startColor = Color.blue;
            AimLine.endColor = Color.blue;
        }
    }

    // ~~~~~ WEAPON SWITCHING ~~~~~
    // ~~~~~ WEAPON SWITCHING ~~~~~

    /*
    public void SelectEquipment(int i)
    {
        if (i == 0)
        {
            // ready to swap main weapon
            doChange_Main = true;
            doChange_Alt = false;
            // UI -- set Main READY and Alt NORMAL
            uiControl.ToggleWeapButton_ReadySelect(true, true);
            uiControl.ToggleWeapButton_ReadySelect(false, false);

        }
        else if (i == 1)
        {
            // ready to swap alt weapon
            doChange_Main = false;
            doChange_Alt = true;
            // UI -- set Main NORMAL and Alt READY
            uiControl.ToggleWeapButton_ReadySelect(false, true);
            uiControl.ToggleWeapButton_ReadySelect(true, false);
        }
    }
    */

    public void SelectWeapon(string weap, bool changeMain)
    {
        Weapon newWeapon;

        switch (weap)
        {
            case "Rifle":
                newWeapon = rifle;
                break;
            case "Repeater":
                newWeapon = repeater;
                break;
            case "Blunderbuss":
                newWeapon = blunderbuss;
                break;
            case "Knife":
                newWeapon = knife;
                break;
            case "Saber":
                newWeapon = saber;
                break;
            case "Hammer":
                newWeapon = hammer;
                break;
            case "Bat":
                newWeapon = bat;
                break;
            case "Mortar":
                newWeapon = mortar;
                break;
            case "Pistol":
                newWeapon = pistol;
                break;
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            default:
                Debug.Log("weapon not implemented");
                newWeapon = nullRanged;
                break;

        }

        if (changeMain)
        {
            mainWeapon = newWeapon;
            mainWeapon.Equip(true);
            UpdateWeapon();
            uiControl.UpdateWeapon_uiPanel(newWeapon, true);
        }
        else
        {
            altWeapon = newWeapon;
            altWeapon.Equip(false);
            UpdateWeapon();
            uiControl.UpdateWeapon_uiPanel(newWeapon, false);
        }
    }

    public void SelectWeapon(Weapon weap, bool changeMain)
    {
        if (changeMain)
        {
            mainWeapon = weap;
            mainWeapon.Equip(true);
            UpdateWeapon();
            //uiControl.UpdateWeapon_uiPanel(weap, true);
        }
        else
        {
            altWeapon = weap;
            altWeapon.Equip(true);
            UpdateWeapon();
            //uiControl.UpdateWeapon_uiPanel(weap, false);
        }
    }

    void UpdateWeapon()
    {
        //uiControl.UpdateWeaponHUD_Main(mainWeapon.weaponName);
        // alt
        //uiControl.UpdateWeaponHUD_Alt(altWeapon.weaponName);
        // distinguish ammo for both? main and alt
    }

    public void UpdateSeraphs() // called in UI manager when window closed
    {
        mainWeapon.SetSeraphs(seraphControl.MainWeapSeraphs);
        altWeapon.SetSeraphs(seraphControl.AltWeapSeraphs);
    }

    IEnumerator OnStart_UpdateSeraphs()
    {
        yield return new WaitForSeconds(0.1f);
        UpdateSeraphs();
    }
}
