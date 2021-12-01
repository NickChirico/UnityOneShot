using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public bool isValidWeapon;
    public bool isPlayerWeapon;
    public bool isMainWeapon;
    public string weaponName;
    public float postureDamage;

    private Seraph_UI[] seraphs;

    public abstract WeaponManager.WeaponType GetWeaponType();

    //public abstract Weapon GetThisWeapon();

    public abstract void Fire(Vector2 origin, Vector2 dir);

    public virtual void Equip(bool isMain)
    {
        isMainWeapon = isMain;
    }

    public void SetSeraphs(List<Seraph_UI> list)
    {
        seraphs = list.ToArray();
    }
    public void ActivateSeraphs(Entity entity, Vector2 pos)
    {
        if (seraphs.Length > 0)
        {
            foreach (Seraph_UI S in seraphs)
            {
                S.mySeraph.StartEffect(entity, pos);
            }
        }
    }
}

#region Ranged Weapon
public class RangedWeapon : Weapon
{
    [Space(5)]
    public int shotDamage;
    public int ammoCapacity;
    public int currentAmmo;
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
    public AudioClip[] shotSounds;
    public AudioClip reload_Sound;
    public LineRenderer ShotTrail_prefab;
    public GameObject ImpactBurst_prefab;
    public float shotVFX_duration;

    private bool hasShot = true;


    public override WeaponManager.WeaponType GetWeaponType()
    {
        return WeaponManager.WeaponType.Ranged;
    }
    public override void Equip(bool b)
    {
        base.Equip(b);
        uiControl.UpdateAmmo(currentAmmo, ammoCapacity, isMainWeapon);
    }

    Ray2D[] rays;
    LineRenderer[] lineTrails;
    Vector2 rayOrigin;

    MovementController moveControl;
    PlayerController playerControl;
    AudioManager audioManager;
    UI_Manager uiControl;

    void Start()
    {
        moveControl = MovementController.GetMoveController;
        playerControl = PlayerController.GetPlayerController;
        audioManager = AudioManager.GetAudioManager;
        uiControl = UI_Manager.GetUIManager;
        currentAmmo = ammoCapacity;
    }


    public bool HasShot()
    {
        return hasShot;
    }
    public void SetHasShot(bool b)
    {
        hasShot = b;
    }

    public override void Fire(Vector2 origin, Vector2 direction)
    {
        rayOrigin = origin;
        if (doCone)
        {
            rays = new Ray2D[coneSpread];
            for (int i = 0; i < rays.Length; i++)
            {
                rays[i] = new Ray2D(rayOrigin, RandomConeAngle(direction));
            }
        }
        else
        {
            rays = new Ray2D[1];
            rays[0] = new Ray2D(rayOrigin, direction * range);
        }

        lineTrails = new LineRenderer[rays.Length];

        // Do the shot(s)
        Shoot(FindHitsFromRays(rays));
        //
        ShotRecoil(-direction);
        //
        LoseAmmo();
        //
        StartCoroutine(ShotEffect());
    }

    private RaycastHit2D[] FindHitsFromRays(Ray2D[] rays)
    {
        RaycastHit2D[] hits = new RaycastHit2D[rays.Length];
        for (int i = 0; i < rays.Length; i++)
        {
            hits[i] = Physics2D.Raycast(rayOrigin, rays[i].direction, range);
        }
        return hits;
    }

    private void Shoot(RaycastHit2D[] hits)
    {
        hasShot = false;
        int i = 0;
        foreach (RaycastHit2D hit in hits)
        {
            LineRenderer line = Instantiate(ShotTrail_prefab, this.transform);
            line.SetPosition(0, rayOrigin);

            if (hit.collider != null)
            {
                line.SetPosition(1, hit.point);
                Vector2 hitPoint = new Vector2(hit.point.x, hit.point.y);

                if (hit.collider.CompareTag("Terrain"))
                {
                    //HitTerrain();
                    Instantiate(ImpactBurst_prefab, hit.point, Quaternion.identity);
                }
                else if (hit.collider.CompareTag("Enemy"))
                {
                    Entity entity = hit.collider.GetComponent<Enemy>();
                    if (entity != null)
                    {
                        ApplyShot(entity, hit.point, hit.distance, shotDamage); // PASS DAMAGE TO ApplyShot(damage);
                        Instantiate(ImpactBurst_prefab, hitPoint, Quaternion.identity);
                    }
                }
                else if (hit.collider.CompareTag("Player") && !isPlayerWeapon)
                {
                    Entity entity = hit.collider.GetComponent<Player>();
                    if (entity != null)
                    {
                        ApplyShot(entity, hit.point, hit.distance, shotDamage); // PASS DAMAGE TO ApplyShot(damage);
                        Instantiate(ImpactBurst_prefab, hitPoint, Quaternion.identity);
                    }
                }
            }
            else
            {
                // Miss!
                Vector2 rayOrigin2D = new Vector2(rayOrigin.x, rayOrigin.y);
                line.SetPosition(1, rayOrigin2D + rays[i].direction * range);
            }
            lineTrails[i] = line;
            i++;
        }


    }

    private void ApplyShot(Entity entityHit, Vector2 hitPoint, float hitDistance, int damage)
    {
        // Determine Damage
        int damageToDeal = damage;

        /*if (doPierce)
        {
            damageToDeal = CalculateProximityDamage(damage, hitDistance);
        }*/

        // Deal Damage
        bool isKillShot = entityHit.TakeDamage(damageToDeal, hitPoint, knockbackForce, postureDamage);

        // Apply Knockback
        // --here

        if(isPlayerWeapon)
        {
            // Speed Boost
            moveControl.SpeedBoost(isKillShot);

            // APPLY SERAPH EFFECTS
            ActivateSeraphs(entityHit, hitPoint);
        }


    }

    public void ShotRecoil(Vector2 dir)
    {
        if (doRecoil)
        {
            moveControl.Recoil(false, dir, recoilForce, recoilDuration);
        }
    }
    public void LoseAmmo()
    {
        currentAmmo--;

        if (isPlayerWeapon)
            SetAmmoUI();
        else if (currentAmmo <= 0)
        {
            
        }
            
    }

    public void SetAmmoUI()
    {
        uiControl.UpdateAmmo(currentAmmo, ammoCapacity, isMainWeapon);
    }

    public void Reload()
    {
        currentAmmo = ammoCapacity;
        uiControl.UpdateAmmo(currentAmmo, ammoCapacity, isMainWeapon);
        SetHasShot(true);
    }

    private IEnumerator ShotEffect()
    {
        audioManager.SetShotSounds(shotSounds);
        audioManager.PlayShotSound(1);
        //Instanciate(GameObject shotEffect);
        yield return new WaitForSeconds(shotVFX_duration);
        DestoryTrailLines();
    }
    public void PlayRechamberSound()
    {
        audioManager.PlayRechamberSound();
    }
    public void PlayFullReloadSound()
    {
        audioManager.SetReloadSound(reload_Sound);
        audioManager.PlayFullReloadSound();
    }

    void DestoryTrailLines()
    {
        foreach (LineRenderer line in lineTrails)
        {
            Destroy(line.gameObject);
        }
    }

    // ~~
    private Vector2 RandomConeAngle(Vector2 dir)
    {
        float x = dir.x;
        float y = dir.y;

        float randomAngle = Random.Range(-coneAngle, coneAngle);

        float angle = randomAngle * Mathf.Deg2Rad;

        float cos = Mathf.Cos(angle);
        float sin = Mathf.Sin(angle);
        float x2 = x * cos - y * sin;
        float y2 = x * sin + y * cos;

        return new Vector2(x2, y2);
    }
}
#endregion


#region Melee Weapon
public class MeleeWeapon : Weapon
{
    [Space(5)]
    public int intervalCount; // -1 for infinite
    public int[] damageArr;
    public float[] knockForceArr;
    public float[] attackDurArr;
    public float[] swingRangeArr;
    public float[] attackRadiusArr;
    public float[] thrustForceArr;
    public float[] thrustDurArr;
    [Space(5)]
    public float[] preDelayArr;
    public float recoverTime;
    public float comboCooldown;
    public float collisionInterval;

    [Space(10)]
    public AudioClip[] swingSounds;

    [Space(10)]
    public GameObject tempAttackDisplay;
    public LayerMask hittableEntity;


    MovementController moveControl;
    AudioManager audioManager;
    Vector2 attackPoint;
    Vector2 direction;
    public int currentInterval = 0;
    public bool canAttack = true;

    public override WeaponManager.WeaponType GetWeaponType()
    {
        return WeaponManager.WeaponType.Melee;
    }

    void Start()
    {
        moveControl = MovementController.GetMoveController;
        audioManager = AudioManager.GetAudioManager;
    }

    public override void Fire(Vector2 origin, Vector2 dir)
    {
        direction = dir;
        float swingRange = swingRangeArr[currentInterval];
        attackPoint = origin + (dir * swingRange);
        SetIndicator(true);
        tempAttackDisplay.transform.position = attackPoint;
    }
    public void SetIndicator(bool b)
    {
        tempAttackDisplay.GetComponent<SpriteRenderer>().enabled = b;
    }

    public void PrepAttack(int interval)
    {
        tempAttackDisplay.GetComponent<SpriteRenderer>().color = Color.yellow;

        float rad = attackRadiusArr[interval];
        tempAttackDisplay.transform.localScale = Vector3.one * rad * 1.25f;
    }

    public void AttackThrust(int interval)
    {
        float dur = thrustDurArr[interval];
        float force = thrustForceArr[interval];

        if (dur != 0 && force != 0)
        {
            moveControl.Thrust(direction, force, dur);
        }

        // Play sound based on attack interval
        audioManager.PlayMeleeSound(interval);
    }

    public void Attack(int interval)
    {
        // TEMP DISPLAY STUFF
        tempAttackDisplay.GetComponent<SpriteRenderer>().color = Color.red;
        Collider2D[] hitEnemies;

        float rad = attackRadiusArr[interval];
        int damageToPass = damageArr[interval];

        tempAttackDisplay.transform.localScale = Vector3.one * (rad * 2); //diameter - TEMP display
        hitEnemies = Physics2D.OverlapCircleAll(attackPoint, rad, hittableEntity);

        if (hitEnemies != null)
        {
            foreach (Collider2D hit in hitEnemies)
            {
                if (hit.CompareTag("Terrain"))
                { }
                else if (hit.CompareTag("Enemy"))
                {
                    Entity entity = hit.GetComponent<Enemy>();
                    if (entity != null)
                    {
                        ApplyAttack(entity, hit.transform.position, damageToPass);
                    }
                }
            }
        }

    }

    private void ApplyAttack(Entity entityHit, Vector2 hitPoint, int damage)
    {
        int damageToDeal = damage;
        // switch( CURRENT ATTACK EFFECT??)
        // { different augments? }
        // ...CalculateProximityDamage(damage, hitDistance)


        entityHit.TakeDamage(damageToDeal, hitPoint, knockForceArr[currentInterval], postureDamage);

        // SERAPH
        ActivateSeraphs(entityHit, hitPoint);
    }

    public void Recover()
    {
        tempAttackDisplay.GetComponent<SpriteRenderer>().color = Color.green;
        tempAttackDisplay.transform.localScale = Vector3.one * 0.25f;
        if (currentInterval < intervalCount - 1)
            currentInterval++;
        else
            ResetAttackSequence();
    }

    public void ResetAttackSequence()
    {
        //Debug.Log("ATTACK COOLDOWN");
        currentInterval = 0;
        tempAttackDisplay.GetComponent<SpriteRenderer>().color = Color.black;
        tempAttackDisplay.transform.localScale = Vector3.one * 0.25f;

        canAttack = false;
        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(comboCooldown);
        canAttack = true;
    }

    public bool CanAttack()
    { return canAttack; }
    public int GetCurrentInterval()
    { return currentInterval; }
}
#endregion

#region SpecialWeapon

public class SpecialWeapon : Weapon
{
    public enum SpecialType { Projectile, Arc, Lunge }
    [Space(5)]
    public SpecialType specialType;  
    public int sp_Damage;
    public int sp_Capacity;
    private int currentAmmo;
    public float sp_Range;
    public float sp_Radius;
    public float sp_Knock;
    public float sp_Duration;
    public float sp_PreDelay;
    public float sp_Cooldown;
    public AnimationCurve arcCurve;
    public bool sp_isArc;
    public float sp_travelTime;
    public Projectile projectile_prefab;
    public GameObject aimIndicator_mortar;

    public AudioClip[] sp_Sounds;

    public override WeaponManager.WeaponType GetWeaponType()
    {
        return WeaponManager.WeaponType.Special;
    }

    public override void Equip(bool b)
    {
        base.Equip(b);
        uiControl.UpdateAmmo(currentAmmo, sp_Capacity, isMainWeapon);
    }

    MovementController moveControl;
    PlayerController playerControl;
    AudioManager audioManager;
    UI_Manager uiControl;

    Vector2 direction;
    Vector2 rayOrigin;

    void Start()
    {
        moveControl = MovementController.GetMoveController;
        playerControl = PlayerController.GetPlayerController;
        audioManager = AudioManager.GetAudioManager;
        uiControl = UI_Manager.GetUIManager;
        currentAmmo = sp_Capacity;
    }

    bool canSpecial = true;
    public bool CanSpecial()
    { return canSpecial; }
    public override void Fire(Vector2 origin, Vector2 dir)
    {
        rayOrigin = origin;
        direction = dir;

        switch (specialType)
        {
            case SpecialType.Projectile:
                if (projectile_prefab != null && currentAmmo > 0)
                    ShootProjectileStraight();
                uiControl.UpdateAmmo(currentAmmo, sp_Capacity, isMainWeapon);
                break;
            case SpecialType.Arc:
                if (projectile_prefab != null)
                    ShootProjectileArc();
                uiControl.UpdateAmmo(currentAmmo, sp_Capacity, isMainWeapon);
                break;
            //
            case SpecialType.Lunge:
                //MoveControl.Recoil(false, MeleeControl.GetDirection(), sp_thrustF, sp_thrustDur);
                //player.Nimble(true);
                break;

            default:
                break;
        }

        if(currentAmmo <= 0)
            StartCoroutine(SpecialCooldown());
    }

    public void ShootProjectileStraight()
    {
        Vector2 dir = playerControl.GetDirection();
        Vector2 origin = playerControl.GetOrigin();

        float rot_z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Projectile bullet = Instantiate(projectile_prefab, origin, Quaternion.Euler(0, 0, rot_z - 90));
        Rigidbody2D rbb = bullet.gameObject.GetComponent<Rigidbody2D>();
        rbb.AddForce(dir * bullet.force_noArc, ForceMode2D.Impulse);
        currentAmmo--;
    }
    public void ShootProjectileArc()
    {
        Vector2 dir = playerControl.GetDirection();
        Vector2 origin = playerControl.GetOrigin();

        float rot_z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Projectile bullet = Instantiate(projectile_prefab, origin, Quaternion.Euler(0, 0, rot_z - 90));

        float distance = (aimIndicator_mortar.transform.position - playerControl.transform.position).magnitude;
        float timeRatio = (distance / sp_Range);
        bullet.SetVals(origin, dir, distance, arcCurve, sp_travelTime, timeRatio);
        currentAmmo--;
    }

    // Helper Functions 
    public void InitArcAim(bool b)
    {
        if (b)
        {
            aimIndicator_mortar.SetActive(true);
            aimIndicator_mortar.transform.position = playerControl.GetOrigin();// + direction*0.5f;
        }
        else
        {
            StartCoroutine(HideArcIndicator());
        }
    }
    public void AimProjectileArc(float ratio)//, Vector2 offset)
    {
        rayOrigin = playerControl.GetOrigin();
        direction = playerControl.GetDirection();

        if (aimIndicator_mortar.activeSelf)
        {
            aimIndicator_mortar.transform.position = Vector2.Lerp(rayOrigin, rayOrigin + direction * sp_Range, ratio);// + offset;
        }
    }
    private IEnumerator HideArcIndicator()
    {
        yield return new WaitForSeconds(0.5f);
        aimIndicator_mortar.SetActive(false);
    }

    // 
    private IEnumerator SpecialCooldown()
    {
        canSpecial = false;
        yield return new WaitForSeconds(sp_Cooldown);
        canSpecial = true;
        currentAmmo = sp_Capacity;
        uiControl.UpdateAmmo(currentAmmo, sp_Capacity, isMainWeapon);
    }
}
#endregion




