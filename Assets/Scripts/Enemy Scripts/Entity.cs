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
    [Range(0.1f, 5)] public float postureDecayRate;
    public float guardBreakDuration;
    public PopUpDamageText damageText;
    public GameObject markedIndicator;
    public GameObject contaminateParticles_prefab;

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
            Die();
            return true;
        }
        else
        {
            return false;
        }
    }

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
                { posture = postureMax + 2; }

                posture = Mathf.Lerp(posture, 0, Time.deltaTime / postureDecayRate);

                if (posture < postureMax / 2)
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

    [Header("AFFLICTIONS")]
    bool isContaminated;
    int contaminateDamage;
    float contaminateDuration;
    float contaminateInterval;
    float contaminateTimer;
    float contaminateTimer_D;
    GameObject particles;

    public void Contaminate(int damage, float duration, float interval)
    {
        isContaminated = true;
        contaminateTimer = 0;
        contaminateDamage = damage;
        contaminateDuration = duration;
        contaminateInterval = interval;
        if (contaminateParticles_prefab != null)
        {
            if (particles == null)
                particles = Instantiate(contaminateParticles_prefab, this.transform);
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

            if (particles != null)
                Destroy(particles);
        }
    }
}
