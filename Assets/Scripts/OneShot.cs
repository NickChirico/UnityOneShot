using System.Collections;
using UnityEngine;

public class OneShot : MonoBehaviour
{
    [Header("Debug Bools")]
    public bool UsingMouse;
    public bool aimLineEnabled;

    [Header("Variables")]
    public int damage = 1;
    public float attackSpeed = 1f;
    public float reloadTime = 0.25f;
    public float weaponRange = 4.5f;
    public float shotDuration = 0.05f;
    public float hitForce;
    public bool ProximityDamageActive;
    public bool ChargeShotActive;
    private bool canShoot;

    [Header("Buffs")]
    public int damageMultiplier = 10;

    private Transform startPoint;
    private PlayerMovement PlayerMove;

    public LineRenderer ShotTrail;
    public LineRenderer AimLine;
    public GameObject ImpactBurst;

    [Header("Audio")]
    public AudioSource GunSource;
    public AudioSource ReloadSource;
    public AudioClip[] shotSounds;
    public AudioClip[] reloadSounds;

    private void Start()
    {
        ShotTrail.enabled = false;
        PlayerMove = this.GetComponent<PlayerMovement>();
        canShoot = true;
    }

    void Update()
    {
        startPoint = this.transform;
        Vector3 direction;

        if (UsingMouse)
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z*-1)); // invert cam Z to make 0
            direction = (targetPos - startPoint.position).normalized;
            
        }
        else
        {
            float lookX, lookY;
            lookX = Input.GetAxis("Mouse X");
            lookY = Input.GetAxis("Mouse Y");
            direction = new Vector3(lookX, lookY, 0).normalized;

        }
        
        //Debug.Log(direction);

        Vector3 lineOrigin = new Vector3(startPoint.position.x, startPoint.position.y + 0.25f, startPoint.position.z);
        // UPDATE AIM LINE
        if (aimLineEnabled)
        {
            if(!AimLine.enabled)
                AimLine.enabled = true;

            AimLine.SetPosition(0, lineOrigin);
            AimLine.SetPosition(1, lineOrigin + (direction * weaponRange));
            if (canShoot)
            {
                AimLine.startColor = Color.yellow;
                AimLine.endColor = Color.yellow;
            }
            else
            {
                AimLine.startColor = Color.red;
                AimLine.endColor = Color.red;
            }

        }
        else
        {
            if (AimLine.enabled)
                AimLine.enabled = false;
        }


        if (Input.GetButtonDown("Fire1") && canShoot)
        {
            StartCoroutine(ShotEffect(shotDuration));
            Shoot(direction);
        }
    }

    // SHOOT ~!~!~!~!
    void Shoot(Vector3 direction)
    {
        Vector3 RayOrigin = new Vector3(startPoint.position.x, startPoint.position.y + 0.25f, startPoint.position.z);

        ShotTrail.SetPosition(0, RayOrigin);
        RaycastHit2D hit = Physics2D.Raycast(RayOrigin, direction, weaponRange);

        if (hit.collider != null) // IF YOU HIT SOMETHING
        {
            ShotTrail.SetPosition(1, hit.point);
            Vector2 hitPoint = new Vector2(hit.point.x, hit.point.y);
            Instantiate(ImpactBurst, hitPoint, Quaternion.identity);


            if (hit.collider.gameObject.name == "ColliderTerrain")
            {
                // IF HIT A WALL

            }
            else
            {
                // IF HIT IS AN ENTITY
                ShootableEntity Entity = hit.collider.GetComponent<ShootableEntity>();
                if (Entity != null) 
                {
                    int damageToDeal;
                    if (ProximityDamageActive)
                    {
                        damageToDeal = CalculateProximityDamage(damage, damageMultiplier, hit.distance);
                    }
                    else
                    {
                        damageToDeal = damage;
                    }

                    HitEntity(Entity, damageToDeal, hitPoint);

                    if (hit.rigidbody != null)
                    {
                        hit.rigidbody.AddForce(-hit.normal * hitForce);
                    }
                }
            }

            //Debug.Log(hit.collider.gameObject.name);
        }
        else
        {
            ShotTrail.SetPosition(1, RayOrigin + (direction * weaponRange));
            //Debug.Log("MISS");
        }

    }

    private int CalculateProximityDamage(int baseDamage, float multiplier, float distance)
    {
        float rangeMultiplier = distance / weaponRange;

        float percent = 2*(1 - (rangeMultiplier));

        float damageMultiplier = percent * multiplier;

        int realDamage = baseDamage + (int)damageMultiplier;


        return realDamage;
    }

    private void HitEntity(ShootableEntity Entity, int damage, Vector2 point)
    {
        PlayerMove.SpeedBoost(Entity.CheckOneShot(damage));

        Entity.TakeDamage(damage, point);

    }

    private void PlayShotSound(int hit)
    {
        switch (hit)
        {
            case 1: // Hit something
                GunSource.clip = shotSounds[Random.Range(0, shotSounds.Length)];
                GunSource.Play();
                break;
            case 2:
                break;

        }
    }

    private void PlayReloadSound()
    {
        ReloadSource.clip = reloadSounds[Random.Range(0, reloadSounds.Length)];
        ReloadSource.Play();
    }

    private IEnumerator ShotEffect(float delay)
    {
        canShoot = false;
        PlayShotSound(1);
        // Instanciate(GameObject shotEffect);

        ShotTrail.enabled = true;
        yield return new WaitForSeconds(delay);
        ShotTrail.enabled = false;

        yield return new WaitForSeconds(attackSpeed);
        PlayReloadSound();
        yield return new WaitForSeconds(reloadTime);
        canShoot = true;
    }
}
