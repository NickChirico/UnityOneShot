using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialController : MonoBehaviour
{
    public static SpecialController _specControl;
    public static SpecialController GetSpecialController { get { return _specControl; } }

    ShotController ShotControl;
    MeleeController MeleeControl;
    MovementController MoveControl;
    Player player;
    SeraphController SeraphControl;

    public GameObject mortarAimIndicator;
    public AnimationCurve aimCurve_arc;

    public enum Special { None, Mark, Burst, Mortar, Smokescreen, Lunge, GreatSlam, SweepBack, Bat }

    [Header("New SPECIAL vars")]
    public Special currentSpecial;
    public string sp_Name;
    public int sp_Damage;
    public int sp_Capacity;
    public float sp_Range;
    public float sp_Radius;
    public float sp_Knock;
    public float sp_Duration;
    public float sp_PreDelay;
    public float sp_travelTime;
    public float sp_Cooldown;
    public AnimationCurve sp_arcCurve;
    public bool sp_isArc;
    public Projectile projectile_prefab;

    // for melee

    //

    // Private vars
    bool canSpecial = true;

    Vector2 direction;
    Vector2 rayOrigin;
    private void Awake()
    {
        _specControl = this;
        player = this.GetComponent<Player>();
    }

    private void Start()
    {
        ShotControl = ShotController.GetShotControl;
        MeleeControl = MeleeController.GetMeleeControl;
        MoveControl = MovementController.GetMoveController;
        SeraphControl = SeraphController.GetSeraphController;

    }
    private void Update()
    {
        direction = ShotControl.GetDirection();
        rayOrigin = ShotControl.GetRayOrigin();
    }


    public void CommenceSpecial(Special spec)
    {
        switch (spec)
        {
            case Special.Mark:
                if (projectile_prefab != null)
                    ShootProjectileStraight();
                break;
            case Special.Burst:
                MoveControl.Recoil(true, Vector2.zero, MoveControl.dashForce / 2.5f, MoveControl.dashDuration / 2);
                StartCoroutine(BurstShots(sp_Capacity));
                break;
            case Special.Mortar:
                if (projectile_prefab != null)
                    ShootProjectileArc();
                break;
            //
            case Special.Lunge:
                //MoveControl.Recoil(false, MeleeControl.GetDirection(), sp_thrustF, sp_thrustDur);
                player.Nimble(true);
                break;
            case Special.Bat:
                MoveControl.Recoil(true, Vector2.zero, 0, sp_Duration);
                MeleeAttack();
                StartCoroutine(HideAttackIndicator());
                break;

            default:
                break;
        }


        StartCoroutine(SpecialCooldown());
    }

    public bool CanSpecial()
    { return canSpecial; }

    public void SetSpecialStats(SpecialWeapon weap)
    {
        sp_Name = weap.weaponName;
        currentSpecial = weap.specialType;
        sp_Damage = weap.sp_Damage;
        sp_Capacity = weap.sp_Capacity;
        sp_Range = weap.sp_Range;
        sp_Radius = weap.sp_Radius;
        sp_Knock = weap.sp_Knock;
        sp_Duration = weap.sp_Duration;
        sp_PreDelay = weap.sp_PreDelay;
        sp_Cooldown = weap.sp_Cooldown;
        sp_isArc = weap.sp_isArc;
        sp_arcCurve = weap.arcCurve;
        sp_travelTime = weap.sp_travelTime;
        projectile_prefab = weap.projectile_prefab;

        Debug.Log(" --Special: " + sp_Name);
    }

    /*public void SetSpecialStats(MeleeWeapon weap)
    {
        currentSpecial = weap.specialType_M;
        sp_Name = weap.sp_Name;
        sp_Damage = weap.sp_Damage;
        sp_Duration = weap.sp_Duration;
        sp_PreDelay = weap.sp_PreDelay;
        sp_Cooldown = weap.sp_Cooldown;
    }*/

    public Special GetCurSpecial()
    { return currentSpecial; }

    // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    // ~~~~~~~~~~ Special: Functions ~~~~~~~~~~~~

    public void ShootProjectileStraight()
    {
        float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Projectile bullet = Instantiate(projectile_prefab, ShotControl.GetRayOrigin(), Quaternion.Euler(0, 0, rot_z - 90));
        Rigidbody2D rbb = bullet.gameObject.GetComponent<Rigidbody2D>();
        rbb.AddForce(direction * bullet.force_noArc, ForceMode2D.Impulse);
    }

    public void ShootProjectileArc()
    {

        float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Projectile bullet = Instantiate(projectile_prefab, rayOrigin, Quaternion.Euler(0, 0, rot_z - 90));

        float distance = (mortarAimIndicator.transform.position - this.transform.position).magnitude;
        float timeRatio = (distance / sp_Range);

        bullet.SetVals(rayOrigin, direction, distance, sp_arcCurve, sp_travelTime, timeRatio);

    }

    public GameObject tempAttackDisplay;
    public LayerMask hittableEntity;
    public void MeleeAttack()
    {
        tempAttackDisplay.SetActive(true);
        //tempAttackDisplay.GetComponent<SpriteRenderer>().color = Color.red;
        Collider2D[] hitEnemies;

        float rad = sp_Radius;
        int damageToPass = sp_Damage;
        Vector2 zone = MeleeControl.GetRayOrigin() + (MeleeControl.GetDirection() * sp_Range);

        tempAttackDisplay.transform.position = zone;
        tempAttackDisplay.transform.localScale = Vector3.one * (rad * 2); //diameter - TEMP display
        hitEnemies = Physics2D.OverlapCircleAll(zone, rad, hittableEntity);

        if (hitEnemies != null)
        {
            foreach (Collider2D hit in hitEnemies)
            {
                if (hit.CompareTag("Terrain"))
                { }
                else if (hit.CompareTag("Enemy"))
                {
                    ShootableEntity entity = hit.GetComponent<ShootableEntity>();
                    if (entity != null)
                    {
                        ApplySpecial(entity, hit.transform.position, damageToPass);
                    }
                }
            }
        }
    }

    public void ApplySpecial(ShootableEntity entity, Vector2 hitPoint, int dam)
    {
        entity.TakeDamage(dam, hitPoint, sp_Knock);


        // SERAPH
        SeraphControl.ActivateSpecWeaponSeraphs(entity, hitPoint);
    }


    // ~~~~~~~~ Special Helper Functions ~~~~~~~~~~~
    public void InitArcAim(bool b)
    {
        if(b)
        { 
            mortarAimIndicator.SetActive(true);
            mortarAimIndicator.transform.position = rayOrigin;// + direction*0.5f;
        }
        else
        {
            StartCoroutine(HideArcIndicator());
        }
    }
    public void AimProjectileArc(float ratio)//, Vector2 offset)
    {
        if(mortarAimIndicator.activeSelf)
        {
            mortarAimIndicator.transform.position = Vector2.Lerp(rayOrigin, rayOrigin + direction * sp_Range, ratio);// + offset;
        }
    }



    public void EndNimble()
    {
        if (player.IsNimble())
            player.Nimble(false);
    }

    //
    private IEnumerator SpecialCooldown()
    {
        canSpecial = false;
        yield return new WaitForSeconds(sp_Cooldown);
        canSpecial = true;        
    }

    private IEnumerator BurstShots(int num)
    {
        for (int i = 0; i < num; i++)
        {
            if (ShotControl.currentAmmo > 0)
                ShotControl.CommenceShot();
            else
                break;
            yield return new WaitForSeconds(sp_Duration / num);
        }
        if(ShotControl.currentAmmo>0)
            ShotControl.SetHasShot(true);
    }

    private IEnumerator HideArcIndicator()
    {
        yield return new WaitForSeconds(0.5f);
        mortarAimIndicator.SetActive(false);

    }
    private IEnumerator HideAttackIndicator()
    {
        yield return new WaitForSeconds(sp_Duration/2f);
        tempAttackDisplay.SetActive(false);

    }

}
