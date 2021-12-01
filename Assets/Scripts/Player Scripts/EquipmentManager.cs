using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager _instanceEquip;
    public static EquipmentManager GetEquipManager { get { return _instanceEquip; } }

    MovementController MoveControl;
    UI_Manager UIControl;

    public enum Weapon { Stave, Rifle, Stave2, Rifle2, None }
    public enum Aphelios { Calibrum, Severum, Gravitum, Infernum, Cresendum }
    public enum GunType { Basic, Charge, Inverse };
    public enum BulletType { Basic, Pierce, Impact };
    public enum DamageEffect { None, Proximity };
    public enum AlternateFire { None, Shotgun, Burst, Flamethrower, MARK };
    public enum SpecialType { None, Roll, Shockwave, Shield };

    [Header("Player Equipment")]
    public Weapon currentWeapon;
    public Aphelios currentAphelios;
    public GunType currentGun;
    public BulletType currentBullet;
    public DamageEffect currentDamageEffect;
    public SpecialType currentSpecial;
    public AlternateFire currentAltFire;

    [Header("Player Stats")]
    public float tempStat;

    [Header("PROTO WEAPON STUFF")]
    public Image calibrumMeter;
    public Image severumMeter;
    public Image gravitumMeter;
    public Image infernumMeter;
    public Image cresendumMeter;


    private void Awake()
    {
        _instanceEquip = this;
    }

    //  Start happens after everything's Awake()
    private void Start()
    {
        MoveControl = MovementController.GetMoveController;
        UIControl = UI_Manager.GetUIManager;

        //calibrumMeter.fillAmount = 0;
        //severumMeter.fillAmount = 0;
        //gravitumMeter.fillAmount = 0;
        //infernumMeter.fillAmount = 0;
        //cresendumMeter.fillAmount = 0;

        UpdateEquipment();

        //UIControl.UpdateCurrentWeaponLabel(currentWeapon.ToString());
    }

    private void Update()
    {
        //
    }

    public void UpdateWeaponMeters(int value)
    {
        switch (value)
        {
            case 1:
                //calibrumMeter.fillAmount = Mathf.Lerp(calibrumMeter.fillAmount, calibrumMeter.fillAmount+0.2f, Time.deltaTime * 8);
                calibrumMeter.fillAmount += 0.2f;
                break;
            case 2:
                //severumMeter.fillAmount = Mathf.Lerp(severumMeter.fillAmount, severumMeter.fillAmount + 0.2f, Time.deltaTime * 8);
                severumMeter.fillAmount += 0.2f;
                break;
            case 3:
                //gravitumMeter.fillAmount = Mathf.Lerp(gravitumMeter.fillAmount, gravitumMeter.fillAmount + 0.2f, Time.deltaTime * 8);
                gravitumMeter.fillAmount += 0.2f;
                break;
            case 4:
                //infernumMeter.fillAmount = Mathf.Lerp(infernumMeter.fillAmount, infernumMeter.fillAmount + 0.2f, Time.deltaTime * 8);
                infernumMeter.fillAmount += 0.2f;
                break;
            case 5:
                //cresendumMeter.fillAmount = Mathf.Lerp(cresendumMeter.fillAmount, cresendumMeter.fillAmount + 0.2f, Time.deltaTime * 8);
                cresendumMeter.fillAmount += 0.2f;
                break;
            default:
                break;
        }
    }

    public void UpdateEquipment()
    {
        /*if (ShotControl.currentGun != currentGun ||
            ShotControl.currentDamageEffect != currentDamageEffect ||
            ShotControl.currentBullet != currentBullet ||
            MoveControl.currentSpecial != currentSpecial ||
            AltFireControl.currentAltFire != currentAltFire)
        {
            MoveControl.UpdateCurrentEquipment(currentSpecial);
            ShotControl.UpdateCurrentEquipment(currentGun, currentDamageEffect, currentBullet);
            AltFireControl.UpdateCurrentEquipment(currentAltFire);
        }*/
    }

    // From Buttons -- Never called from code

    public void SetWeapon(Weapon w)
    {
        if (w == Weapon.Rifle)
        {
            //ShotControl.aimLineEnabled = true;
            //MeleeControl.tempAttackDisplay.GetComponent<SpriteRenderer>().enabled = false; // TEMP TODO: better indicator
        }
        else if (w == Weapon.Stave)
        {
            //ShotControl.aimLineEnabled = false;
            //MeleeControl.tempAttackDisplay.GetComponent<SpriteRenderer>().enabled = true;
        }
        currentWeapon = w;
        //UIControl.UpdateCurrentWeaponLabel(currentWeapon.ToString());
    }
    public void SetGun(GunType gun)
    {
        currentGun = gun;
    }
    public void SetBullet(BulletType bullet)
    {
        currentBullet = bullet;
    }
    public void SetAlternate(AlternateFire alt)
    {
        currentAltFire = alt;
    }
    public void SetSpecial(SpecialType spec)
    {
        currentSpecial = spec;
    }
}
