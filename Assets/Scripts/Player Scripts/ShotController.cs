using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    public static ShotController _shotControl;
    public static ShotController GetShotControl { get { return _shotControl; } }

    public EquipmentManager.GunType currentGun;
    public EquipmentManager.DamageEffect currentDamageEffect;
    public EquipmentManager.BulletType currentBullet;

    [Header("Debug Options")]
    public bool UsingMouse;
    public bool aimLineEnabled;
    public bool rangeIndicatorEnabled;

    [Header("Components")]
    public LineRenderer ShotTrail;
    public LineRenderer AimLine;
    public GameObject ImpactBurst;
    public GameObject ImpactPierceBurst;
    private MovementController moveController;
    private AltShotController altController;
    private AudioManager audioManager;

    [Header("Range Indicator")]
    public LineRenderer RangeIndicator;
    public float ThetaScale = 0.01f;
    private int circleSize;
    private float Theta;

    [Header("Variable Stats")]
    public int baseDamage = 10;
    public float weaponRange = 4.5f;
    public float shotDuration = 0.5f;
    public float shotEffectDuration = 0.08f;
    public float reloadDuration = 1f;
    public float delayFromAlt = 0.65f;
    public float hitForce = 400f;
    public float pierceDamageFalloff = 0.1f;
    public float chargeRate = 2f;
    public float chargeLimit = 100f;


    // Private variables
    private bool hasShot;
    private float altDelayTimer;
    private float chargeLevel;
    private bool charging;

    private Vector3 direction;
    private Vector3 rayOrigin;

    private void Awake()
    {
        currentGun = EquipmentManager.GunType.Basic;
        currentDamageEffect = EquipmentManager.DamageEffect.None;
        currentBullet = EquipmentManager.BulletType.Basic;
        _shotControl = this;
    }

    private void Start()
    {
        moveController = MovementController.GetMoveController;
        altController = AltShotController.GetAltControl;
        audioManager = AudioManager.GetAudioManager;

        ShotTrail.gameObject.SetActive(true);
        RangeIndicator.gameObject.SetActive(true);
        AimLine.gameObject.SetActive(true);
        ShotTrail.enabled = false;
        hasShot = true;
    }

    public bool HasShot()
    {
        return hasShot;
    }
    public void SetHasShot(bool b)
    {
        hasShot = b;
    }

  ////
  //// ~~ UPDATE ~~
    private void Update()
    {
        if (UsingMouse)
        {
            // Mouse Look Controls
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z * -1)); // invert cam Z to make 0
            direction = (targetPos - this.transform.position).normalized;
        }
        else
        {
            // Controller/Non Mouse Controls
            direction = Vector3.zero;
        }

        // Aim Line
        rayOrigin = new Vector3(this.transform.position.x,
            this.transform.position.y + 0.25f,
            this.transform.position.z);
        UpdateAimLine(aimLineEnabled, rangeIndicatorEnabled, direction);

        // Charging Shot
        if (charging && chargeLevel < chargeLimit)
        {
            chargeLevel += Time.deltaTime * chargeRate;

            if (chargeLevel > chargeLimit * 0.05f && !audioManager.ChargeSource.isPlaying)
                audioManager.PlayChargeSound();
        }
    }

    void UpdateAimLine(bool enabled, bool circleEnabled, Vector3 direction)
    {
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

    // Update current equipment - called from EquipmentManager
    public void UpdateCurrentEquipment(EquipmentManager.GunType gun, EquipmentManager.DamageEffect amplifier, EquipmentManager.BulletType bullet)
    {
        currentGun = gun;
        currentDamageEffect = amplifier;
        currentBullet = bullet;
    }

    public Vector3 GetDirection()
    {
        return direction;
    }
    public Vector3 GetStartpoint()
    {
        return rayOrigin;
    }
    public void ToggleAimLine()
    {
        if (aimLineEnabled)
            aimLineEnabled = false;
        else
            aimLineEnabled = true;
    }

    ////
    //// ~~ SHOOT ~~
    public void ButtonPress()
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
                Shoot(direction, baseDamage);
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
                Shoot(direction, (int)chargeDamage);
                chargeLevel = 0;
                break;

            case "Inverse":
                // ...bullet comes back
                break;

            default:
                // Basic --> Do nothing
                break;
        }
    }

    private void Shoot(Vector2 direction, int damage)
    {
        hasShot = false;
        ShotTrail.SetPosition(0, rayOrigin);

        //RaycastHit2D hit = 

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, weaponRange);
        RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, direction, weaponRange);

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
                            hit.rigidbody.AddForce(knockBack);
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
                                h.rigidbody.AddForce(knockBack);
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
            ShotTrail.SetPosition(1, rayOrigin2 + (direction * weaponRange));
        }

        StartCoroutine(ShotEffect());
        altController.ResetShotBuffer();
    }

    private void ApplyShot(ShootableEntity entityHit, Vector2 hitPoint, float hitDistance, int damage)
    {
        // Determine Damage
        int damageToDeal = damage;
        switch (currentDamageEffect.ToString())
        {
            case "None":
                //damageToDeal = damage;
                break;

            case "Proximity":
                damageToDeal = CalculateProximityDamage(damage, hitDistance);
                break;
        }

        // Deal Damage
        bool isKillShot = entityHit.TakeDamage(damageToDeal, hitPoint);

        // Speed Boost
        moveController.SpeedBoost(isKillShot);
    }

    /*private void HitTerrain()
    {
    }*/

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

    ////
    ////~~ RELOAD 
    public void PlayReloadSound()
    {
        audioManager.PlayReloadSound();
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

  ////
  ////~~ Coroutines ~~
    private IEnumerator ShotEffect()
    {
        audioManager.PlayShotSound(1);
        //Instanciate(GameObject shotEffect);

        ShotTrail.enabled = true;
        yield return new WaitForSeconds(shotEffectDuration);
        ShotTrail.enabled = false;
    }
}
