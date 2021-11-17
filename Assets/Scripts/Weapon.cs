using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public string weaponName;

    //public abstract Weapon GetThisWeapon();
}

public class RangedWeapon : Weapon
{
    [Space(5)]
    public int shotDamage;
    public int ammoCapacity;
    public float range;
    public float reloadDuration;
    public float knockbackForce;
    [Space(10)]
    public bool doRecoil;
    public float recoilForce, recoilDuration;
    [Space(10)]
    public float delayBetweenShots;
    public bool doRechamber;
    public float rechamberDuration;
    [Space(10)]
    public bool doCone;
    public int coneSpread;
    public float coneAngle;

    [Space(10)]
    public SpecialController.Special specialType_R;
    public string sp_Name;
    public int sp_Damage;
    public int sp_Capacity;
    public float sp_Range;
    public float sp_Duration;
    public float sp_PreDelay;
    public float sp_Cooldown;
    public AnimationCurve arcCurve;
    public bool sp_isArc;
    public float sp_travelTime;
    public Projectile projectile_prefab;

    [Space(10)]
    public AudioClip[] shotSounds;
    public AudioClip[] sp_Sounds;
    public AudioClip reload_Sound;


}


    public class MeleeWeapon : Weapon
{
    [Space(5)]
    public int intervalCount; // -1 for infinite
    public int[] damageArr;
    public float[] attackDurArr;
    public float[] swingRangeArr;
    public float[] attackRadiusArr;
    public float[] thrustForceArr;
    public float[] thrustDurArr;
    [Space(5)]
    public float[] preDelayArr;
    public float recoverTime;
    public float comboCooldown;

    [Space(10)]
    public SpecialController.Special specialType_M;
    public string sp_Name;
    public int sp_Damage;
    public float sp_Duration;
    public float sp_PreDelay;
    public float sp_Cooldown;

    [Space(10)]
    public AudioClip[] swingSounds;
    public AudioClip[] sp_Sounds;

}

// ~~~~~~~~~~~




