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

    public enum WeaponType { Ranged, Melee, Special }
    public enum WeaponE { None, Saber, Hammer, Halberd, Bludgeon, Rifle, Repeater, Blunderbuss, Spewer, Mark, Mortar }
    public WeaponE SelectedWeapon;
    public enum SpecialE { None, Mark, Shotgun, Mortar, Lunge, Bat }
    public SpecialE SelectedSpecial;

    RangedWeapon rifle;
    RangedWeapon repeater;
    RangedWeapon blunderbuss;
    MeleeWeapon knife;
    MeleeWeapon hammer;
    SpecialWeapon mark;
    SpecialWeapon shotgun;
    SpecialWeapon mortar;
    SpecialWeapon lunge;
    SpecialWeapon bat;

    public WeaponSlot mainWeaponSlot;

    public RangedWeapon currentR;
    public MeleeWeapon currentM;
    public SpecialWeapon currentS;
    bool isMelee;

    /*public RangedWeapon weapon1R;
    public MeleeWeapon weapon1M;
    public SpecialWeapon weapon1S;

    public RangedWeapon weapon2R;
    public MeleeWeapon weapon2M;
    public SpecialWeapon weapon2S;*/

    private void Awake()
    {
        _weapManager = this;

        rifle = this.GetComponent<Rifle>();
        blunderbuss = this.GetComponent<Blunderbuss>();
        repeater = this.GetComponent<Repeater>();

        knife = this.GetComponent<Knife>();
        hammer = this.GetComponent<Hammer>();

        mark = this.GetComponentInChildren<sp_Mark>();
        // shotgun
        mortar = this.GetComponentInChildren<sp_Mortar>();
        // lunge
        //  bat = this.GetComponentInChildren<Bat>();
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
            }
        }
        else
        {
            if (currentR != null)
            {
                // Pass ranged weapon stats to ShotController
                ShotControl.SetWeaponStats(currentR);
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
        UIControl.UpdateWeaponHUD_Main(weapName);
    }

    public void UpdateSpecial()
    {
        if (currentS != null)
        {
            SpecControl.SetSpecialStats(currentS);
            UIControl.UpdateCurrentSpecialPanelTMP(SelectedSpecial.ToString());
            UIControl.UpdateCurrentSpecialLabel(currentS.weaponName);
        }
    }

    public void EquipWeapon(int index)
    {
        if (currentR != null)
        {
            ShotControl.SetWeaponStats(currentR);
            ToggleAimVisuals(false);
        }
        if (currentM != null)
        {
            MeleeControl.SetWeaponStats(currentM);
            ToggleAimVisuals(true);
        }
        //if (currentS != null)
        //  SpecControl.SetWeaponStats(currentS);
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

    public void SelectSpecial(string s)
    {
        switch (s)
        {
            case "Mark":
                SelectedSpecial = SpecialE.Mark;
                currentS = mark;
                break;
            case "Shotgun":
                SelectedSpecial = SpecialE.Shotgun;
                currentS = shotgun;
                break;
            case "Mortar":
                SelectedSpecial = SpecialE.Mortar;
                currentS = mortar;
                break;
            case "Lunge":
                SelectedSpecial = SpecialE.Lunge;
                currentS = lunge;
                break;
            case "Bat":
                SelectedSpecial = SpecialE.Bat;
                currentS = bat;
                break;
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            default:
                Debug.Log("special not implemented");
                break;
        }
        UpdateSpecial();
    }

    public void SetMainWeapon()
    {
        if (mainWeaponSlot.GetWeapon() != null)
        {
            SetWeapon(1, mainWeaponSlot.GetWeapon());
        }
    }
    public void SetWeapon(int index, Weapon_DragUI weap)
    {
        RangedWeapon ranged = null;
        MeleeWeapon melee = null;
        SpecialWeapon special = null;

        switch (weap.weaponName)
        {
            case "Saber":
                melee = knife;
                isMelee = true;
                break;
            case "Hammer":
                melee = hammer;
                isMelee = true;
                break;
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            case "Rifle":
                ranged = rifle;
                isMelee = false;
                break;
            case "Repeater":
                ranged = repeater;
                isMelee = false;
                break;
            case "Blunderbuss":
                ranged = blunderbuss;
                isMelee = false;
                break;
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            case "Mark":
                special = mark;
                isMelee = false;
                break;
            case "Mortar":
                special = mortar;
                isMelee = false;
                break;
        }

        if (index == 1)
        {
            currentM = melee;
            currentR = ranged;
            currentS = special;
        }
        else if (index == 2)
        {
            
        }

        EquipWeapon(1);
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

    // ~~~~~~~~~~~~~~

    public void GoToSpots()
    {
        Weapon_DragUI[] AllWeapons = FindObjectsOfType<Weapon_DragUI>();
        foreach (Weapon_DragUI w in AllWeapons)
        {
            w.GoToSpot();
        }
    }
}
