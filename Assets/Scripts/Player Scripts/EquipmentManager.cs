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
    ShotController ShotControl;
    AltShotController AltFireControl;

    public enum GunType { Basic, Charge, Inverse };
    public enum BulletType { Basic, Pierce, Impact };
    public enum DamageEffect { None, Proximity };
    public enum AlternateFire { None, Shotgun, Burst, Flamethrower };
    public enum SpecialType { None, Roll, Shockwave, Shield };



    [Header("Player Equipment")]
    public GunType currentGun;
    public BulletType currentBullet;
    public DamageEffect currentDamageEffect;
    public SpecialType currentSpecial;
    public AlternateFire currentAltFire;

    [Header("Player Stats")]
    public float tempStat;


    private void Awake()
    {
        _instanceEquip = this;
    }

    //  Start happens after everything's Awake()
    private void Start()
    {
        MoveControl = MovementController.GetMoveController;
        ShotControl = ShotController.GetShotControl;
        AltFireControl = AltShotController.GetAltControl;
    }

    private void Update()
    {
        //
    }

    public void UpdateEquipment()
    {
        if (ShotControl.currentGun != currentGun ||
            ShotControl.currentDamageEffect != currentDamageEffect ||
            ShotControl.currentBullet != currentBullet ||
            MoveControl.currentSpecial != currentSpecial ||
            AltFireControl.currentAltFire != currentAltFire)
        {
            MoveControl.UpdateCurrentEquipment(currentSpecial);
            ShotControl.UpdateCurrentEquipment(currentGun, currentDamageEffect, currentBullet);
            AltFireControl.UpdateCurrentEquipment(currentAltFire);
        }
    }

    // From Buttons -- Never called from code
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
