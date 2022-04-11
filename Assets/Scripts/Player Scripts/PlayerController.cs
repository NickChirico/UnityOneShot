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
    public Player myPlayer;

    public string[] EquippedWeapons;

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
        myPlayer = gameObject.GetComponent<Player>();
        nullRanged = FindObjectOfType<null_ranged>();
        nullMelee = FindObjectOfType<null_melee>();
        nullSpec = FindObjectOfType<null_special>();
        

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        try { myMasterDictionary = GameObject.Find("Master Dictionary").GetComponent<MasterDictionary>(); }
        catch { myMasterDictionary = null; }
        if (myMasterDictionary != null)
        {
            foreach (var temp in myMasterDictionary.weapons)
            {
                print("this is happening");
                if (temp.GetWeaponType() == WeaponManager.WeaponType.Melee)
                {
                    print("and now this is happening");
                    temp.gameObject.GetComponent<MeleeWeapon>().tempAttackDisplay = GameObject.Find("Melee Indicator");
                }
            }
        }
        uiControl = GameObject.Find("*** UI Manager").GetComponent<UI_Manager>();
        seraphControl = SeraphController.GetSeraphController;
    }

    void Start()
    {
        EquippedWeapons = new string[2] { mainWeapon.weaponName, altWeapon.weaponName };


        //UpdateWeapon();
        DetermineAimLine();

        StartCoroutine(OnStart_UpdateSeraphs());
    }

    // Update is called once per frame
    void Update()
    {
        direction = UpdateDirection(usingMouse);
        rayOrigin = UpdateRayOrigin();

        if (aimlineEnabled)
        {
            UpdateAimLine(true, direction);
        }
        else
        {
            if (AimLine.enabled)
                AimLine.enabled = false;
        }
        /*if (mainWeapon.GetWeaponType() == WeaponManager.WeaponType.Ranged || altWeapon.GetWeaponType() == WeaponManager.WeaponType.Ranged)
        {
            UpdateAimLine(true, direction);
        }
        else
        { UpdateAimLine(false, direction); }*/

        if (playerInputActions.Player.Interact.triggered && myPlayer.GetInteractStatus())
        {
            print("interaction is happening");
            myPlayer.InteractWith();
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
        this.transform.position.y,
        this.transform.position.z);
    }

    public Vector2 GetOrigin() { return rayOrigin; }
    public Vector2 GetDirection() { return direction; }

    bool aimlineEnabled;
    float indicatorRange;
    float segmentRange;
    void DetermineAimLine()
    {
        if (mainWeapon == null || altWeapon == null)
            return;

        float mainRange;
        float altRange;

        if (mainWeapon.isValidWeapon && altWeapon.isValidWeapon)
        {
            if (mainWeapon.IsRanged() && altWeapon.IsRanged()) // IF BOTH WEAPONS ARE RANGED
            {
                mainRange = mainWeapon.GetComponent<RangedWeapon>().range;
                altRange = altWeapon.GetComponent<RangedWeapon>().range;
                if (mainRange > altRange)
                    indicatorRange = mainRange;
                else if (mainRange < altRange)
                    indicatorRange = altRange;
                aimlineEnabled = true;
            }
            else if (mainWeapon.IsRanged())
            {
                mainRange = mainWeapon.GetComponent<RangedWeapon>().range;
                indicatorRange = mainRange;
                aimlineEnabled = true;
            }
            else if (altWeapon.IsRanged())
            {
                altRange = altWeapon.GetComponent<RangedWeapon>().range;
                indicatorRange = altRange;
                aimlineEnabled = false;
            }
            else
            { aimlineEnabled = false; }
        }
    }

    void UpdateAimLine(bool enabled, Vector2 dir)
    {
        if (enabled)
        {
            if (!AimLine.enabled)
                AimLine.enabled = true;


            AimLine.SetPosition(0, rayOrigin);

            AimLine.SetPosition(1, rayOrigin + (dir.normalized * indicatorRange)); //NORMALIZED for static length.
           
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
        //print("I hope master dictionary is awake for this");
        if (myMasterDictionary == null)
        {
            print("master dictionary is missing");
        }
        if (weap == null)
        {
            print("weapon string is null");
        }
        if (myMasterDictionary.WeaponDictionary == null)
        {
            print("weapon dictionary is null");
        }
        myMasterDictionary.WeaponDictionary.TryGetValue(weap, out var newWeapon);
        if (uiControl == null)
        {
            print("ui control is missing");
        }
        if (newWeapon != null)
        {
            if (changeMain)
            {
                mainWeapon = newWeapon;
                mainWeapon.Equip(true);
                UpdateWeapon();
                //EquippedWeapons[0] = mainWeapon.weaponName;
                uiControl.UpdateWeapon_uiPanel(mainWeapon, true);
            }
            else
            {
                altWeapon = newWeapon;
                altWeapon.Equip(false);
                UpdateWeapon();
                //EquippedWeapons[1] = altWeapon.weaponName;
                uiControl.UpdateWeapon_uiPanel(altWeapon, false);
            }
        }
        else
        {
            print("Weapon is null!");
        }

        DetermineAimLine();
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


    // ~~~~~~~~~~~~~~~~~~~~~
    // WEAPON DROP - SWAPPING
    string weapDropName;
    GameObject inspectedWeapDrop;
    public bool CanSwapWeapon = false;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("weaponDrop"))
        {
            inspectedWeapDrop = collision.gameObject;
            weapDropName = inspectedWeapDrop.GetComponent<WeaponDrop>().GetWeaponName();
            CanSwapWeapon = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("weaponDrop"))
        {
            inspectedWeapDrop = null;
            weapDropName = null;
            CanSwapWeapon = false;
        }
    }

    public void SwapMain()
    {
        if (inspectedWeapDrop != null)
        {
            DropCurrentWeapon(true);
            SelectWeapon(weapDropName, true);
            Destroy(inspectedWeapDrop);
        }
    }
    public void SwapAlt()
    {
        if (inspectedWeapDrop != null)
        {
            DropCurrentWeapon(false);
            SelectWeapon(weapDropName, false);
            Destroy(inspectedWeapDrop);
        }
    }

    [Header("Weapon Drop Prefabs")]
    public GameObject RIFLE_DROP;
    public GameObject REPEATER_DROP;
    public GameObject SHOTGUN_DROP;
    public GameObject KNIFE_DROP;
    public GameObject SABER_DROP;
    public GameObject MORTAR_DROP;

    public void DropCurrentWeapon(bool isMain)
    {
        string weaponToDrop;

        if (isMain)
            weaponToDrop = mainWeapon.weaponName;
        else
            weaponToDrop = altWeapon.weaponName;

        Vector3 offset = new Vector2(Random.Range(0, 0.3f), Random.Range(0, 0.3f));
        switch (weaponToDrop)
        {
            case "Rifle":
                Instantiate(RIFLE_DROP, this.transform.position + offset, Quaternion.identity);
                break;
            case "Repeater":
                Instantiate(REPEATER_DROP, this.transform.position + offset, Quaternion.identity);
                break;
            case "Blunderbuss":
                Instantiate(SHOTGUN_DROP, this.transform.position + offset, Quaternion.identity);
                break;
            case "Knife":
                Instantiate(KNIFE_DROP, this.transform.position + offset, Quaternion.identity);
                break;
            case "Saber":
                Instantiate(SABER_DROP, this.transform.position + offset, Quaternion.identity);
                break;
            case "Mortar":
                Instantiate(MORTAR_DROP, this.transform.position + offset, Quaternion.identity);
                break;
            default:
                Debug.Log("No Weapon Dropped ???");
                break;
        }
    }
}
