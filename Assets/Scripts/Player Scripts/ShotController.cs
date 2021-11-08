using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    public static ShotController _shotControl;
    public static ShotController GetShotControl { get { return _shotControl; } }

    #region ## PARAMETERS ##
    private PlayerInputActions playerInputActions;
    public enum Special { None, Projectile, Burst }

    public EquipmentManager.GunType currentGun;
    public EquipmentManager.DamageEffect currentDamageEffect;
    public EquipmentManager.BulletType currentBullet;
    public EquipmentManager.Aphelios currentAphelios;

    [Header("Debug Options")]
    public bool usingMouse;
    public bool aimLineEnabled;
    public bool rangeIndicatorEnabled;

    [Header("Components")]
    public LineRenderer ShotTrail;
    public LineRenderer AimLine;
    public GameObject ImpactBurst;
    public GameObject ImpactPierceBurst;
    private MovementController moveControl;
    private AltShotController altControl;
    private AudioManager audioManager;
    private UI_Manager uiControl;

    [Header("Range Indicator")]
    public LineRenderer RangeIndicator;
    public float ThetaScale = 0.01f;
    private int circleSize;
    private float Theta;

    [Header("Variable Stats")]
    public string weaponName;
    public int baseDamage = 10;
    public int ammoCapacity = 6;
    [HideInInspector] public int currentAmmo;
    public float weaponRange = 4.5f;
    public float shotDuration = 0.5f;
    public float shotEffectDuration = 0.08f;
    public float reloadDuration = 1f;
    public float rechamberDuration = 0.5f;
    public float delayFromAlt = 0.65f;
    public float hitForce = 400f;
    public float pierceDamageFalloff = 0.1f;
    public float chargeRate = 2f;
    public float chargeLimit = 100f;

    [Header("Proto Stats")]
    public float calibRange = 5f;
    public float severRange = 3f;
    public float graviRange = 4f;
    public float inferRange = 4f;
    public float creseRange = 3.5f;

    // Private variables
    private bool hasShot;
    private float altDelayTimer;
    private float chargeLevel;
    private bool charging;

    private Vector3 direction;
    private Vector3 rayOrigin;

    // NEW VARIABLES
    [Header("NEW WEAPON VARS")]
    public bool doRechamber = true;
    public float delayBetweenShots;
    public bool doCone = false;
    public int coneSpreadCount;
    public float coneAngle;

    public bool doHold = false;
    public bool doPierce = false;
    public bool doRecoil = false;
    public float recoilForce, recoilDuration;

    public LineRenderer ShotTrail_prefab;
    public GameObject ImpactBurst_prefab;

    #endregion

    #region ## SET UP ##
    private void Awake()
    {
        currentGun = EquipmentManager.GunType.Basic;
        currentDamageEffect = EquipmentManager.DamageEffect.None;
        currentBullet = EquipmentManager.BulletType.Basic;
        currentAphelios = EquipmentManager.Aphelios.Calibrum;
        _shotControl = this;

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    private void Start()
    {
        moveControl = MovementController.GetMoveController;
        altControl = AltShotController.GetAltControl;
        audioManager = AudioManager.GetAudioManager;
        uiControl = UI_Manager.GetUIManager;

        ShotTrail.gameObject.SetActive(true);
        RangeIndicator.gameObject.SetActive(true);
        AimLine.gameObject.SetActive(true);
        ShotTrail.enabled = false;
        hasShot = true;

        currentAmmo = ammoCapacity;
        uiControl.UpdateAmmo(currentAmmo, ammoCapacity);
    }

    public bool HasShot()
    {
        return hasShot;
    }
    public void SetHasShot(bool b)
    {
        hasShot = b;
    }

    public void SetWeaponStats(RangedWeapon weap)
    {
        weaponName = weap.weaponName;

        baseDamage = weap.shotDamage;
        weaponRange = weap.range;
        reloadDuration = weap.reloadDuration;

        delayBetweenShots = weap.delayBetweenShots;
        doRechamber = weap.doRechamber;
        rechamberDuration = weap.rechamberDuration;

        doCone = weap.doCone;
        coneSpreadCount = weap.coneSpread;
        coneAngle = weap.coneAngle;

        doRecoil = weap.doRecoil;
        recoilForce = weap.recoilForce;
        recoilDuration = weap.recoilDuration;

        ammoCapacity = weap.ammoCapacity;
        currentAmmo = ammoCapacity;
        uiControl.UpdateAmmo(currentAmmo, ammoCapacity);

        audioManager.SetShotSounds(weap.shotSounds);
        audioManager.SetReloadSound(weap.reload_Sound);

        Debug.Log("switched to: " + weaponName);
    }
    #endregion

    // ~~~~~~~~~~~~~~~~~~~~~~~~~~
    #region ### Update and actions ###
    private void Update()
    {
        // Aim Direction -- direction
        direction = UpdateDirection(usingMouse);
        // Aim Line -- rayOrigin
        UpdateAimLine(aimLineEnabled, rangeIndicatorEnabled, direction);

        // Charging Shot
        if (charging && chargeLevel < chargeLimit)
        {
            chargeLevel += Time.deltaTime * chargeRate;

            if (chargeLevel > chargeLimit * 0.05f && !audioManager.ChargeSource.isPlaying)
                audioManager.PlayChargeSound();
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
    void UpdateAimLine(bool enabled, bool circleEnabled, Vector3 direction)
    {
        rayOrigin = new Vector3(this.transform.position.x,
        this.transform.position.y + 0.25f,
        this.transform.position.z);

        if (enabled)
        {
            if (!AimLine.enabled)
                AimLine.enabled = true;

            AimLine.SetPosition(0, rayOrigin);
            AimLine.SetPosition(1, rayOrigin + (direction * weaponRange));

            if (circleEnabled)
            {
                if (!RangeIndicator.enabled)
                    RangeIndicator.enabled = true;

                Theta = 0f;
                circleSize = (int)((1f / ThetaScale) + 1f);
                RangeIndicator.positionCount = circleSize;
                for (int i = 0; i < circleSize; i++)
                {
                    Theta += (2.0f * Mathf.PI * ThetaScale);
                    float x = rayOrigin.x + weaponRange * Mathf.Cos(Theta);
                    float y = rayOrigin.y + weaponRange * Mathf.Sin(Theta);
                    RangeIndicator.SetPosition(i, new Vector3(x, y, 0));
                }
            }
            else
            {
                if (RangeIndicator.enabled)
                    RangeIndicator.enabled = false;
            }
        }
        else
        {
            if (AimLine.enabled)
                AimLine.enabled = false;
        }
    }
    public Vector3 GetDirection()
    {
        return direction;
    }
    public Vector3 GetRayOrigin()
    {
        return rayOrigin;
    }

    // Update current equipment - called from EquipmentManager
    public void UpdateCurrentEquipment(EquipmentManager.GunType gun, EquipmentManager.DamageEffect amplifier, EquipmentManager.BulletType bullet)
    {
        currentGun = gun;
        currentDamageEffect = amplifier;
        currentBullet = bullet;
    }

    public void SetAimLine(bool b)
    {
        aimLineEnabled = b;
    }
    public void ToggleAimLine()
    {
        if (aimLineEnabled)
            aimLineEnabled = false;
        else
            aimLineEnabled = true;
    }

    #endregion

    //// ~~ SHOOT [for charge] (old - remove) ~~
    /*public void ButtonPress()
    {
        switch (currentGun.ToString())
        {
            case "Charge": // CHARGE GUN
                charging = true;
                audioManager.PlayChargeSound();
                break;

            case "Inverse": // INVERSE GUN
                // bullet shoots out...
                break;

            default:
                // Basic
                ProtoFire();
                // -- is really --> Shoot();
                break;
        }
    }
    public void ButtonRelease()
    {
        switch (currentGun.ToString())
        {
            case "Charge":
                charging = false;
                audioManager.StopChargeSound();
                float chargeDamage = ((chargeLevel / chargeLimit) * baseDamage) + baseDamage;
                Shoot(direction, (int)chargeDamage, weaponRange);
                chargeLevel = 0;
                break;

            case "Inverse":
                // ...bullet comes back
                break;

            default:
                // Basic --> Do nothing
                break;
        }
    }*/
    /*private void ProtoFire()
    {
        switch (currentAphelios)
        {
            case EquipmentManager.Aphelios.Calibrum:
                Shoot(direction, baseDamage, calibRange);
                break;
            case EquipmentManager.Aphelios.Severum:
                Shoot(direction, baseDamage, severRange);
                break;
            case EquipmentManager.Aphelios.Gravitum:
                ShootLock(direction, baseDamage, graviRange);
                break;
            case EquipmentManager.Aphelios.Infernum:
                ShootLock(direction, baseDamage, inferRange);
                break;
            case EquipmentManager.Aphelios.Cresendum:
                ShootLock(direction, baseDamage, creseRange);
                break;
            default:
                Shoot(direction, baseDamage, weaponRange);
                break;
        }
    }*/

    // ~~~~~~~~~~~~~~~~~~~~~~~~~~
    #region ## SHOT ##
    Ray2D[] rays;
    LineRenderer[] lineTrails;
    public void CommenceShot()
    {

        if (doCone)
        {
            rays = new Ray2D[coneSpreadCount];
            for (int i = 0; i < rays.Length; i++)
            {
                rays[i] = new Ray2D(rayOrigin, RandomConeAngle(direction));
            }
        }
        else
        {
            rays = new Ray2D[1];
            rays[0] = new Ray2D(rayOrigin, direction * weaponRange);


            //Shoot(direction, baseDamage, weaponRange);
        }

        lineTrails = new LineRenderer[rays.Length];

        // Do the shot(s)
        Shoot(FindHitsFromRays(rays));
        //
        ShotRecoil();
        //
        LoseAmmo();
        //
        StartCoroutine(ShotEffect());
    }

    private void ShootOLD(Vector2 direction, int damage, float range)
    {
        hasShot = false;
        ShotTrail.SetPosition(0, rayOrigin);

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, range);
        RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, direction, range);

        bool pierceShot = false;
        switch (currentBullet.ToString())
        {
            case "Basic":
                pierceShot = false;
                break;

            case "Pierce":
                pierceShot = true;
                break;

            case "Impact":
                pierceShot = false;
                break;
        }

        if (hit.collider != null)
        {
            // If Single Target (not Pierce) Shot
            if (!pierceShot)
            {
                ShotTrail.SetPosition(1, hit.point);
                Vector2 hitPoint = new Vector2(hit.point.x, hit.point.y);

                if (hit.collider.CompareTag("Terrain"))
                {
                    //HitTerrain();
                }
                else if (hit.collider.CompareTag("Enemy"))
                {
                    ShootableEntity entity = hit.collider.GetComponent<ShootableEntity>();
                    if (entity != null)
                    {
                        ApplyShot(entity, hit.point, hit.distance, damage);
                        Instantiate(ImpactBurst, hitPoint, Quaternion.identity);

                        if (hit.rigidbody != null)
                        {
                            Vector2 knockBack = CalculateKnockBack(-hit.normal, hitForce);
                            //hit.rigidbody.AddForce(knockBack);
                        }
                    }

                }
            }
            else // Pierce Shot
            {
                float damageToPass = damage;
                foreach (RaycastHit2D h in hits)
                {
                    if (h.collider.CompareTag("Terrain"))
                    {
                        ShotTrail.SetPosition(1, h.point);
                        Instantiate(ImpactBurst, h.point, Quaternion.identity);
                        //HitTerrain();
                        break;
                    }
                    else if (h.collider.CompareTag("Enemy"))
                    {
                        ShootableEntity e = h.collider.GetComponent<ShootableEntity>();
                        if (e != null)
                        {
                            ApplyShot(e, h.point, h.distance, (int)damageToPass);
                            damageToPass -= damageToPass * pierceDamageFalloff;
                            Instantiate(ImpactPierceBurst, h.point, Quaternion.identity);

                            if (h.rigidbody != null)
                            {
                                Vector2 knockBack = CalculateKnockBack(-h.normal, hitForce);
                                //h.rigidbody.AddForce(knockBack);
                            }
                        }
                    }
                }
                ShotTrail.SetPosition(1, hits[hits.Length - 1].point);
            }
        }
        else
        {
            // MISS Shot
            Vector2 rayOrigin2 = new Vector2(rayOrigin.x, rayOrigin.y);
            ShotTrail.SetPosition(1, rayOrigin2 + (direction * range));
        }

        StartCoroutine(ShotEffect());
        altControl.ResetShotBuffer();
    }

    private RaycastHit2D[] FindHitsFromRays(Ray2D[] rays)
    {
        RaycastHit2D[] hits = new RaycastHit2D[rays.Length];
        for (int i = 0; i < rays.Length; i++)
        {
            hits[i] = Physics2D.Raycast(rayOrigin, rays[i].direction, weaponRange);
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
                    ShootableEntity entity = hit.collider.GetComponent<ShootableEntity>();
                    if (entity != null)
                    {
                        ApplyShot(entity, hit.point, hit.distance, baseDamage); // PASS DAMAGE TO ApplyShot(damage);
                        Instantiate(ImpactBurst_prefab, hitPoint, Quaternion.identity);
                    }
                }
                else // other entities hit...
                { }
            }
            else
            {
                // Miss!
                Vector2 rayOrigin2D = new Vector2(rayOrigin.x, rayOrigin.y);
                line.SetPosition(1, rayOrigin2D + rays[i].direction * weaponRange);
            }
            lineTrails[i] = line;
            i++;
        }


    }

    private void ApplyShot(ShootableEntity entityHit, Vector2 hitPoint, float hitDistance, int damage)
    {
        // Determine Damage
        int damageToDeal = damage;

        if (doPierce)
        {
            damageToDeal = CalculateProximityDamage(damage, hitDistance);
        }

        // Deal Damage
        bool isKillShot = entityHit.TakeDamage(damageToDeal, hitPoint);

        // Apply Knockback
        // --here
        // APPLY KNOCKBACK
        // APPLY KNOCKBACK
        // APPLY KNOCKBACK

        // Speed Boost
        moveControl.SpeedBoost(isKillShot);

        // Bonus Effects
        /*
        // APPLY PROXIMITY
        // APPLY PROXIMITY et al

        switch (currentDamageEffect.ToString())
        {
            case "None":
                //damageToDeal = damage;
                break;

            case "Proximity":
                damageToDeal = CalculateProximityDamage(damage, hitDistance);
                break;
        }

        switch (entityHit.GetDropType())
        {
            case ShootableEntity.DropType.Green:
                EquipmentManager.GetEquipManager.UpdateWeaponMeters(1);
                break;
            ...
            ...
        }*/
    }

    /*private void HitTerrain()
    {
    }*/
    #endregion

    // ~~~~~~~~~~~~~~~~~~~~~~~~~~
    #region ### Calculation Helper Functions ###
    public Vector2 CalculateKnockBack(Vector2 direction, float force)
    {
        Vector2 knockback = direction * force;

        return knockback;
    }

    public int CalculateProximityDamage(int damage, float distance)
    {
        float multiplier = damage / 2;
        float percent = 2 * (1 - (distance / weaponRange));
        float damageMultiplier = percent * multiplier;
        int realDamage = (int)(damage - multiplier + damageMultiplier);
        return realDamage;
    }
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

    public void ShotRecoil()
    {
        if (doRecoil)
        {
            moveControl.Recoil(false, -direction, recoilForce, recoilDuration);
        }
    }
    #endregion

    // ~~~~~~~~~~~~~~~~~~~~~~~~~~
    #region ### Ammo and HUD UI ###
    public void LoseAmmo()
    {
        currentAmmo--;
        UpdateAmmoUI();
    }

    public void UpdateAmmoUI()
    {
        uiControl.UpdateAmmo(currentAmmo, ammoCapacity);
    }
    #endregion

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~
    #region ### Reloading ###
    public void PlayReloadSound()
    {
        audioManager.PlayRechamberSound();
    }
    public void PlayFullReloadSound()
    {
        audioManager.PlayFullReloadSound();
    }

    public void ToggleAimLineColor(bool isReloading)
    {
        if (isReloading)
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

    public void AltFireAction()
    {
        altDelayTimer = delayFromAlt;
    }
    #endregion

    // ~~~~~~~~~~~~~~~~~~~~~~~~~~
    #region ### Coroutines ###
    private IEnumerator ShotEffect()
    {
        audioManager.PlayShotSound(1);
        //Instanciate(GameObject shotEffect);
        yield return new WaitForSeconds(shotEffectDuration);
        DestoryTrailLines();
    }
    void DestoryTrailLines()
    {
        foreach (LineRenderer line in lineTrails)
        {
            Destroy(line.gameObject);
        }
    }





    #endregion 
}
