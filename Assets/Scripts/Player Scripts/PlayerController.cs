using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController _Control;
    public static PlayerController GetPlayerController { get { return _Control; } }

    public bool usingMouse = true;

    public Weapon mainWeapon;
    public Weapon altWeapon;
    //public Weapon altWeapon; // "set Weapon" script
    [Space(10)]

    public RangedWeapon rangedWeap1;
    public MeleeWeapon meleeWeap1;
    public SpecialWeapon specWeap1;
    public RangedWeapon rangedWeap2;
    public MeleeWeapon meleeWeap2;
    public SpecialWeapon specWeap2;

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
        uiControl = UI_Manager.GetUIManager;
        UpdateWeapon();
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
        if (weap != null)
        {
            weap.Fire(rayOrigin, direction);
        }
    }

    private void FireMainWeapon() // OLD
    {
        if (rangedWeap1 != null)
        {
            rangedWeap1.Fire(rayOrigin, direction);
        }
    }

    private void FireAltWeapon() // OLD
    {
        if (meleeWeap1 != null)
        {
            meleeWeap1.Fire(rayOrigin, direction);
        }
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

    void UpdateAimLine(bool enabled, Vector2 direction)
    {
        float indicatorRange;
        float segmentRange;
        if (enabled)
        {
            if (!AimLine.enabled)
                AimLine.enabled = true;

            AimLine.SetPosition(0, rayOrigin);

            if (rangedWeap1.isValidWeapon)
            {
                if (rangedWeap2.isValidWeapon)
                {
                    if (rangedWeap1.range > rangedWeap2.range)
                    {
                        indicatorRange = rangedWeap1.range;
                        segmentRange = rangedWeap2.range;
                    }
                    else
                    {
                        indicatorRange = rangedWeap2.range;
                        segmentRange = rangedWeap1.range;
                    }
                }
                else
                {
                    indicatorRange = rangedWeap1.range;
                    segmentRange = rangedWeap1.range;
                }
            }
            else if (rangedWeap2.isValidWeapon)
            {
                indicatorRange = rangedWeap2.range;
                segmentRange = rangedWeap2.range;
            }
            else
            {
                indicatorRange = 2f;
                segmentRange = 2f;
            }

            AimLine.SetPosition(1, rayOrigin + (direction * segmentRange));
            AimLine.SetPosition(2, rayOrigin + (direction * indicatorRange));
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
            AimLine.startColor = Color.yellow;
            AimLine.endColor = Color.yellow;
        }
    }

    // ~~~~~ WEAPON SWITCHING ~~~~~
    // ~~~~~ WEAPON SWITCHING ~~~~~
    private bool doChange_Main;
    private bool doChange_Alt;

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

    public void SelectWeapon(string weap)
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
            case "Mark":
                newWeapon = mark;
                break;
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            default:
                Debug.Log("weapon not implemented");
                newWeapon = nullRanged;
                break;

        }

        if (doChange_Main)
        {
            mainWeapon = newWeapon;
            mainWeapon.Equip();
            UpdateWeapon();
            uiControl.UpdateWeapon_uiPanel(newWeapon, true);
            doChange_Main = false;
        }
        else if (doChange_Alt)
        {
            altWeapon = newWeapon;
            altWeapon.Equip();
            UpdateWeapon();
            uiControl.UpdateWeapon_uiPanel(newWeapon, false);
            doChange_Alt = false;
        }
    }

    void UpdateWeapon()
    {
        bool isMelee = false;
        // main
        if (mainWeapon is RangedWeapon)
        {
            rangedWeap1 = (RangedWeapon)mainWeapon;
            meleeWeap1 = nullMelee;
            specWeap1 = nullSpec;
        }
        else if (mainWeapon is MeleeWeapon)
        {
            meleeWeap1 = (MeleeWeapon)mainWeapon;
            rangedWeap1 = nullRanged;
            specWeap1 = nullSpec;
            isMelee = true;
        }
        else if (mainWeapon is SpecialWeapon)
        {
            specWeap1 = (SpecialWeapon)mainWeapon;
            rangedWeap1 = nullRanged;
            meleeWeap1 = nullMelee;
        }
        uiControl.UpdateWeaponHUD_Main(mainWeapon.weaponName, isMelee);

        // alt

        if (altWeapon is RangedWeapon)
        {
            rangedWeap2 = (RangedWeapon)altWeapon;
            meleeWeap2 = nullMelee;
            specWeap2 = nullSpec;
            isMelee = false;
        }
        else if (altWeapon is MeleeWeapon)
        {
            meleeWeap2 = (MeleeWeapon)altWeapon;
            rangedWeap2 = nullRanged;
            specWeap2 = nullSpec;
            isMelee = true;
        }
        else if (altWeapon is SpecialWeapon)
        {
            specWeap2 = (SpecialWeapon)altWeapon;
            meleeWeap2 = nullMelee;
            rangedWeap2 = nullRanged;
        }
        uiControl.UpdateWeaponHUD_Alt(altWeapon.weaponName, isMelee);
        // distinguish ammo for both? main and alt

    }

    public void UpdateSeraphs() // called in UI manager when window closed
    {
        mainWeapon.SetSeraphs(seraphControl.MainWeapSeraphs);
        altWeapon.SetSeraphs(seraphControl.AltWeapSeraphs);
    }
}
