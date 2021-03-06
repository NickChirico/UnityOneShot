using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    [Header("ENTITY")]
    public int MaxHealth;
    public int currentHealth;

    public float postureMax;
    public float posture;
    [Range(1f, 6f)] public float postureRechargeRate;
    public float guardBreakDuration;
    public PopUpDamageText damageText;
    public GameObject markedIndicator;
    public GameObject contaminateParticles_prefab;
    public GameObject stormParticles_prefab;

    [Space(5)]
    public HealthBar healthBar;
    public PostureBar postureBar;

    public bool guardBroken;
    bool vulnerable;
    bool canTakeDamage;
    float markTimer;
    float markDuration;

    public virtual void Start()
    {

        canTakeDamage = true;
        currentHealth = MaxHealth;
        try
        {
            //mySpawner = FindObjectOfType<EnemySpawner>();
        }
        catch
        {
            //thisEnemy = null;
        }
        SetMarkIndicator(false);

        if (postureBar != null && healthBar != null)
        {
            postureBar.SetVals(postureMax);
            healthBar.SetVals(MaxHealth);
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (vulnerable)
        {
            if (markTimer < markDuration)
            {
                markTimer += Time.deltaTime;
            }
            else
            {
                vulnerable = false;
                SetMarkIndicator(false);
            }
        }

        if (isContaminated) // CONTAMINATE -- variables and methods at bottom of class
            UpdateContaminate();

        if (isCharged) // LIGHTNING SURGE -- seraph vars and methods at bottom
            UpdateLightning();
        //
        UpdatePosture();
        UpdateHealth();
    }

    // PLAYER - TakeDamage
    public virtual void TakeDamage(Entity attacker, int damageAmount, float postureDamage)
    {
        currentHealth -= damageAmount;
        posture += postureDamage;

        Vector2 hitpoint = new Vector2(this.transform.position.x, this.transform.position.y + 0.35f);
        PopUpDamageText T = Instantiate(damageText, hitpoint, Quaternion.identity);
        T.SendMessage("SetTextRun", damageAmount);

        // EVENTUALLY, REMOVE THIS 'TAKEDAMAGE'
        // -- BOTH PLAYER AND ENEMIES SHOULD CALL THE SAME 'TAKEDAMAGE' BELOW...
    }

    // ENEMY - TakeDamage
    public virtual bool TakeDamage(int damageAmount, Vector2 damageSpot, float knockForce, float postureDamage)
    {
        if (canTakeDamage)
        {
            //canTakeDamage = false;
            //StartCoroutine(Invuln());
            // --- removed temporarily cus it messed up shotgun
            // - may not be necessary,
            // -  unless melee attacks are hitting the same enemy too many times (melee.collisionInterval)

            if (vulnerable)
            {
                damageAmount *= 2;
                vulnerable = false;
                SetMarkIndicator(false);
            }

            posture += postureDamage;
            if (posture >= postureMax && !guardBroken)
            {
                GuardBreak();
            }

            if (guardBroken)
            {
                float f = (float)damageAmount * 1.25f;
                damageAmount = (int)f;
            }

            currentHealth -= damageAmount;
            if (healthBar != null) // HP BAR
                healthBar.SetHealth((float)currentHealth/(float)MaxHealth);

            PopUpDamageText T = Instantiate(damageText, damageSpot, Quaternion.identity);
            T.SendMessage("SetTextRun", damageAmount);
        }

        if (currentHealth <= 0)
        {
            if (canDie)
            {
                canDie = false;
                Die();
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    bool canDie = true;
    public virtual void Die()
    {
        //gameObject.SetActive(false);
        //if (mySpawner != null)
        //    mySpawner.CheckEnemiesAlive();

        if(this.tag != "Player")
            Destroy(this.gameObject);
    }

    public virtual void UpdateHealth()
    {
        
    }

    public virtual void UpdatePosture()
    {
        if (postureBar != null)
        {
            if (posture > 0)
            {
                if (posture > postureMax)
                { posture = postureMax; }

                //posture = Mathf.Lerp(posture, 0, Time.deltaTime / postureDecayRate);
                posture -= postureRechargeRate * 0.01f;

                if (posture < postureMax / 4)
                {
                    if (guardBroken)
                    {
                        guardBroken = false;
                        postureBar.GuardBreak(false);
                    }
                }
            }
           
            postureBar.SetPosture(posture);
        }
    }

    public virtual void GuardBreak()
    {
        if (postureBar != null)
        {
            guardBroken = true;
            postureBar.GuardBreak(true);
        }
    }

    public virtual void Stun()
    {
        Debug.Log("ENTITY stun");
    }

    public virtual bool IsPlayer()
    { return false; }
    public virtual bool IsEnemy()
    { return false; }


    //   ~~~~~~~~~ Seraph Afflications ~~~~~~~~~~
    public void Mark(float dur)
    {
        vulnerable = true;
        SetMarkIndicator(true);
        markDuration = dur;
        markTimer = 0;
    }

    private void SetMarkIndicator(bool b)
    {
        if (markedIndicator != null)
            markedIndicator.SetActive(b);
    }

    [Header("CONTAMINATE")]
    bool isContaminated;
    int contaminateDamage;
    float contaminateDuration;
    float contaminateInterval;
    float contaminateTimer;
    float contaminateTimer_D;
    GameObject contam_particles;

    public void Contaminate(int damage, float duration, float interval)
    {
        isContaminated = true;
        contaminateTimer = 0;
        contaminateDamage = damage;
        contaminateDuration = duration;
        contaminateInterval = interval;
        if (contaminateParticles_prefab != null)
        {
            if (contam_particles == null)
                contam_particles = Instantiate(contaminateParticles_prefab, this.transform);
        }
    }

    private void UpdateContaminate() // called in update if 'isContaminated'
    {
        if (contaminateTimer < contaminateDuration)
        {
            contaminateTimer += Time.deltaTime;

            if (contaminateTimer_D < contaminateInterval)
            {
                contaminateTimer_D += Time.deltaTime;
            }
            else
            {
                TakeDamage(contaminateDamage, this.transform.position, 0, 0);
                contaminateTimer_D = 0;
            }
        }
        else
        {
            isContaminated = false;

            if (contam_particles != null)
                Destroy(contam_particles);
        }
    }

    [Header("LIGHTNING SURGE")]
    bool isCharged;
    float lightningTimer;
    float lightningDuration;
    int lightningDamage;
    float lightningKnockForce;
    float lightningPostureDamage;
    bool lightning_doStun;
    GameObject storm_particles;
    GameObject bolt_object;

    public void StormStrike(float dur, int boltDamage, bool doStun, GameObject boltObject)
    {
        lightningTimer = dur;
        lightningDamage = boltDamage;
        bolt_object = boltObject;
        lightning_doStun = doStun;
        isCharged = true;

        if (stormParticles_prefab != null)
        {
            if (storm_particles == null)
                storm_particles = Instantiate(stormParticles_prefab, this.transform);
        }
    }
    public bool IsCharged()
    { return isCharged; }
    private void UpdateLightning()
    {
        if (lightningTimer > 0)
        {
            lightningTimer -= Time.deltaTime;
        }
        else
        {
            FireLightning();
        }
    }
    private void FireLightning()
    {
        if (lightning_doStun)
            Stun();

        TakeDamage(lightningDamage, this.transform.position, lightningKnockForce, lightningPostureDamage);
        isCharged = false;

        if (bolt_object != null)
        {
            Instantiate(bolt_object, this.transform.position, Quaternion.identity);
        }

        if (storm_particles != null)
            Destroy(storm_particles);
    }
    public void AccelerateLightning(float amount)
    {
        lightningTimer -= amount;
    }
}
