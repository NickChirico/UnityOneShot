using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager _weapManager;
    public static WeaponManager GetWeaponManager { get { return _weapManager; } }

    MovementController MoveControl;
    ShotController ShotControl;
    MeleeController MeleeControl;
    SpecialController SpecControl;
    UI_Manager UIControl;
    //PlayerStateManager SM;

    public enum WeaponE { None, Saber, Hammer, Halberd, Bludgeon, Rifle, Repeater, Blunderbuss, Spewer }
    public WeaponE SelectedWeapon;

    RangedWeapon rifle;
    RangedWeapon repeater;
    RangedWeapon blunderbuss;
    MeleeWeapon knife;
    MeleeWeapon hammer;

    public RangedWeapon currentR;
    public MeleeWeapon currentM;
    bool isMelee;

    private void Awake()
    {
        _weapManager = this;

        rifle = this.GetComponent<Rifle>();
        blunderbuss = this.GetComponent<Blunderbuss>();
        repeater = this.GetComponent<Repeater>();

        knife = this.GetComponent<Knife>();
        hammer = this.GetComponent<Hammer>();
    }

    private void Start()
    {
        MoveControl = MovementController.GetMoveController;
        ShotControl = ShotController.GetShotControl;
        SpecControl = SpecialController.GetSpecialController;
        MeleeControl = MeleeController.GetMeleeControl;
        UIControl = UI_Manager.GetUIManager;
        //SM = FindObjectOfType<PlayerStateManager>();

        //SelectWeapon("Rifle");
    }

    public void UpdateWeapon()
    {
        if (isMelee)
        {
            if (currentM != null)
            {
                // Pass melee weapon stats to MeleeController
                MeleeControl.SetWeaponStats(currentM);
                SpecControl.SetSpecialStats(currentM);
            }
        }
        else
        {
            if (currentR != null)
            {
                // Pass ranged weapon stats to ShotController
                ShotControl.SetWeaponStats(currentR);
                SpecControl.SetSpecialStats(currentR);
            }
        }
        ToggleAimVisuals(isMelee);
        UIControl.UpdateCurrentWeaponPanelTMP(SelectedWeapon.ToString());

        // -- getting weapon name -little sloppy
        string weapName = "";
        if (currentR != null)
            weapName = currentR.weaponName;
        else if (currentM != null)
            weapName = currentM.weaponName;
        //
        UIControl.UpdateCurrentWeaponLabel(weapName, IsMelee());
    }

    public void SelectWeapon(string w)
    {
        switch (w)
        {
            case "Saber":
                SelectedWeapon = WeaponE.Saber;
                isMelee = true; currentM = knife; currentR = null;
                break;
            case "Hammer":
                SelectedWeapon = WeaponE.Hammer;
                isMelee = true; currentM = hammer; currentR = null;
                break;
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            case "Rifle":
                SelectedWeapon = WeaponE.Rifle;
                isMelee = false; currentR = rifle; currentM = null;
                break;
            case "Repeater":
                SelectedWeapon = WeaponE.Repeater;
                isMelee = false; currentR = repeater; currentM = null;
                break;
            case "Blunderbuss":
                SelectedWeapon = WeaponE.Blunderbuss;
                isMelee = false; currentR = blunderbuss; currentM = null;
                break;
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            default:
                Debug.Log("weapon not implemented");
                break;

        }
        UpdateWeapon();
    }
    public bool IsMelee()
    {
        return isMelee;
    }
    private void ToggleAimVisuals(bool melee)
    {
        MeleeControl.SetIndicator(melee);
        ShotControl.SetAimLine(!melee);
    }
}
