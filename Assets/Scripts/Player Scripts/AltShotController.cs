using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltShotController : MonoBehaviour
{
    public static AltShotController _altControl;
    public static AltShotController GetAltControl { get { return _altControl; } }

    public EquipmentManager.AlternateFire currentAltFire;

    [Header("Components")]
    private ShotController shot;
    private MovementController move;
    private AudioManager audioManager;

    [Header("Variable Stats")]
    public float delayFromShot;

    private int baseDamageAlt;
    private float weaponRangeAlt;
    private float knockbackForceAlt;
    private float recoilForce;
    private float recoilDuration;
    private float effectDurationAlt;
    private float reloadDurationAlt;
    private float putAwayTime;
    private float manaCost;
    private LineRenderer AltTrail_Prefab;
    private GameObject AltImpactBurst_Prefab;

    private float altFireRate;

    [Header("Shotgun")]
    public int shotgunBaseDamage;
    public float shotgunFireRate;
    public float shotgunRange;
    public float shotgunKnockbackForce;
    public float shotgunRecoilForce;
    public float shotgunRecoilDuration;
    public float shotgunReloadDuration;
    public float shotgunPutAwayTime;
    public float shotgunManaCost;
    public float shotgunEffectDuration;
    public LineRenderer TrailPrefab_Shotgun;
    public GameObject ImpactBurst_Shotgun;
    [Space(10)]
    public int shotgunSpreadCount;
    public float angleAmount;

    [Header("Burst")]
    public int burstBaseDamage;
    public float burstFireRate;
    public float burstRange;
    public float burstKnockbackForce;
    public float burstDashForce;
    public float burstDashDuration;
    public float burstReloadDuration;
    public float burstPutAwayTime;
    public float burstManaCost;
    public float burstEffectDuration;
    public LineRenderer TrailPrefab_Burst;
    public GameObject ImpactBurst_Burst;
    [Space(10)]
    public int shotsPerBurst;
    public float delayBtwnBurst;

    [Header("Mana Regen")]
    public float maxMana;
    public float currentMana;
    public float manaRegenRate;
    public float manaRegenDelay;
    private float manaRegenTimer;

    // Private variables
    private bool holdingAltFire;
    private float reloadTimerAlt;
    private float shotDelayTimer;

    private Ray2D[] rays;
    private Vector3 rayOrigin3D;
    private Vector2 rayOrigin2D;
    private Vector2 direction;
    private LineRenderer[] lineTrails;

    private void Awake()
    {
        currentAltFire = EquipmentManager.AlternateFire.None;
        _altControl = this;
    }

    void Start()
    {
        move = MovementController.GetMoveController;
        shot = ShotController.GetShotControl;
        audioManager = AudioManager.GetAudioManager;

        reloadTimerAlt = altFireRate;
        shotDelayTimer = delayFromShot;
    }

    void Update()
    {
        UpdateManaRegen();

        // If using mouse...{}
        // calculate Direction and RayOrigin
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z * -1)); // invert cam Z to make 0
        direction = (targetPos - this.transform.position).normalized;

        rayOrigin3D = new Vector3(this.transform.position.x,
            this.transform.position.y + 0.25f,
            this.transform.position.z);
        rayOrigin2D = new Vector2(rayOrigin3D.x, rayOrigin3D.y);

        // Shot Delay Check
        if (shotDelayTimer < delayFromShot)
        {
            shotDelayTimer += Time.deltaTime;
        }
        // Fire Rate Check
        if (reloadTimerAlt < altFireRate)
        {
            reloadTimerAlt += Time.deltaTime;
        }
    }
    
    public bool CanAltFire()
    {
        return (reloadTimerAlt >= altFireRate) && (currentMana >= manaCost) && (shotDelayTimer >= delayFromShot); 
    }
    public float GetPutAwayTime()
    { return putAwayTime; }

    public void UpdateCurrentEquipment(EquipmentManager.AlternateFire altFire)
    {
        currentAltFire = altFire;

        switch (currentAltFire.ToString())
        {
            case "Shotgun":
                altFireRate = shotgunFireRate;
                baseDamageAlt = shotgunBaseDamage;
                weaponRangeAlt = shotgunRange;
                knockbackForceAlt = shotgunKnockbackForce;
                recoilForce = shotgunRecoilForce;
                recoilDuration = shotgunRecoilDuration;
                reloadDurationAlt = shotgunReloadDuration;
                putAwayTime = shotgunPutAwayTime;
                manaCost = shotgunManaCost;
                effectDurationAlt = shotgunEffectDuration;
                AltTrail_Prefab = TrailPrefab_Shotgun;
                AltImpactBurst_Prefab = ImpactBurst_Shotgun;
                break;

            case "Burst":
                altFireRate = burstFireRate;
                baseDamageAlt = burstBaseDamage;
                weaponRangeAlt = burstRange;
                knockbackForceAlt = burstKnockbackForce;
                recoilForce = burstDashForce;
                recoilDuration = burstDashDuration;
                reloadDurationAlt = burstReloadDuration;
                putAwayTime = burstPutAwayTime;
                manaCost = burstManaCost;
                effectDurationAlt = burstEffectDuration;
                AltTrail_Prefab = TrailPrefab_Burst;
                AltImpactBurst_Prefab = ImpactBurst_Burst;
                break;

            case "Flamethrower":
                break;

            default:
                break;
        }
    }

    private void UpdateManaRegen()
    {
        if (manaRegenTimer >= manaRegenDelay)
        {
            RegenMana();
        }
        else
        {
            if (manaRegenTimer < manaRegenDelay)
                manaRegenTimer += Time.deltaTime;
        }
    }
    private void RegenMana()
    {
        if (currentMana < maxMana)
        {
            if (currentMana < 0)
                currentMana = 0;

            currentMana += Time.deltaTime * manaRegenRate;
        }
        else if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }
    }

    public void CommenceAltFire()
    {
        holdingAltFire = true;
        switch (currentAltFire.ToString())
        {
            case "Shotgun":
                AltFire();
                break;

            case "Burst":
                StartCoroutine(BurstFire());
                break;

            default:
                break;
        }
        currentMana -= manaCost;
        manaRegenTimer = 0;
        reloadTimerAlt = 0;
    }
    public void ToggleHoldingAlt(bool b)
    { holdingAltFire = b; }

    public void AltFire()
    {
        // Determine Rays for AltFire Type
        switch (currentAltFire.ToString())
        {
            case "Shotgun":
                rays = new Ray2D[shotgunSpreadCount];
                lineTrails = new LineRenderer[rays.Length];
                for (int i = 0; i < rays.Length; i++)
                {
                    rays[i] = new Ray2D(rayOrigin3D, RandomShotgunAngle(direction));
                }
                break;

            case "Burst":
                rays = new Ray2D[] { new Ray2D(rayOrigin2D, direction * weaponRangeAlt) };
                lineTrails = new LineRenderer[rays.Length];
                break;

            case "Flamethrower":
                break;

            default: // None
                break;

        }
        // All the hit logic...
        ProcessHits(FindHitsFromRays(rays));
        //
        StartCoroutine(AltShotEffect());
    }

    public void AltMovement()
    {
        Vector2 dir = Vector2.zero;
        switch (currentAltFire.ToString())
        {
            case "Shotgun":
                move.Recoil(false, -direction, recoilForce, recoilDuration);
                break;

            case "Burst":
                move.Recoil(true, Vector2.zero, recoilForce, recoilDuration);
                break;

            default:
                break;
        }
    }

    private RaycastHit2D[] FindHitsFromRays(Ray2D[] rays)
    {
        RaycastHit2D[] hits = new RaycastHit2D[rays.Length];

        for (int i = 0; i < rays.Length; i++)
        {
            hits[i] = Physics2D.Raycast(rayOrigin3D, rays[i].direction, weaponRangeAlt);
        }

        return hits;
    }

    private void ProcessHits(RaycastHit2D[] hits)
    {
        int i = 0;
        foreach (RaycastHit2D hit in hits)
        {
            LineRenderer line = Instantiate(AltTrail_Prefab, this.transform);
            line.SetPosition(0, rayOrigin3D);

            if (hit.collider != null)
            {
                // Its a Hit!

                line.SetPosition(1, rayOrigin2D + rays[i].direction * hits[i].distance);
                Instantiate(AltImpactBurst_Prefab, hit.point, Quaternion.identity);

                if (hit.collider.CompareTag("Terrain"))
                {
                    //HitTerrain();
                }
                else if (hit.collider.CompareTag("Enemy"))
                {
                    ShootableEntity e = hit.collider.GetComponent<ShootableEntity>();
                    if (e != null)
                    {
                        ApplyAltFire(e, hit.point, hit.distance, baseDamageAlt);

                        if (hit.rigidbody != null)
                        {
                            hit.rigidbody.AddForce(-hit.normal * knockbackForceAlt);
                        }
                    }
                }
                else
                { }
            }
            else
            {
                // Miss!
                line.SetPosition(1, rayOrigin2D + rays[i].direction * weaponRangeAlt);
            }
            lineTrails[i] = line;
            i++;
        }
    }

    private void ApplyAltFire(ShootableEntity entityHit, Vector2 hitPoint, float hitDist, int damage)
    {
        // Determine Damage
        int damageToDeal = damage;

        switch (currentAltFire.ToString())
        {
            case "Shotgun":
                damageToDeal = shot.CalculateProximityDamage(damage, hitDist);
                break;

            case "Burst":
                damageToDeal = damage;
                break;

            case "Flamethrower":
                break;

            default:
                break;
        }

        // Deal Damage
        bool isKillShot = entityHit.TakeDamage(damageToDeal, hitPoint);
        // DO SOMETHING WITH KillShot?
    }

    private Vector2 RandomShotgunAngle(Vector2 dir)
    {
        float x = dir.x;
        float y = dir.y;

        float randomAngle = Random.Range(-angleAmount, angleAmount);

        float angle = randomAngle * Mathf.Deg2Rad;

        float cos = Mathf.Cos(angle);
        float sin = Mathf.Sin(angle);
        float x2 = x * cos - y * sin;
        float y2 = x * sin + y * cos;

        return new Vector2(x2, y2);
    }

    // Shot Delay - called from Shot when you shoot
    public void ResetShotBuffer()
    {
        shotDelayTimer = 0;
    }

    //~~ RELOAD & ShotEffectCoroutine ~~

    private IEnumerator AltShotEffect()
    {

        audioManager.StopAltSound();
        audioManager.PlayAltFireSound(currentAltFire.ToString(), true);
        //..Instanciate(GameObject shotEffect);

        yield return new WaitForSeconds(effectDurationAlt);
        DestoryTrailLines();
    }

    private IEnumerator BurstFire()
    {
        for (int i = 0; i < shotsPerBurst; i++)
        {
            AltFire();
            yield return new WaitForSeconds(delayBtwnBurst);
        }
    }

    void DestoryTrailLines()
    {
        foreach (LineRenderer line in lineTrails)
        {
            Destroy(line.gameObject);
        }
    }
}
