using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableEntity : MonoBehaviour
{
    public EnemySpawner mySpawner;
    public PopUpDamageText damageText;
    public enum DropType { Green, Red, Purple, Blue, White };
    public DropType thisDropType;

    private int health;
    private int currentHealth;
    private float invulnTime;

    private bool canTakeDamage;
    private bool marked;

    Enemy thisEnemy;

    public GameObject markedIndicator;
    float markTimer;
    float markDuration;

    public GameObject contaminateParticles_prefab;

    private void Start()
    {
        canTakeDamage = true;
        currentHealth = health; // temp for 123 spawn in enemies
        try 
        {
            mySpawner = FindObjectOfType<EnemySpawner>();
            thisEnemy = this.GetComponent<Enemy>();
        }
        catch
        {
            thisEnemy = null;
            mySpawner = null;
        }

        SetMarkIndicator(false);
    }

    private void Update()
    {
        if (marked)
        {
            if (markTimer < markDuration)
            {
                markTimer += Time.deltaTime;
            }
            else
            {
                marked = false;
                SetMarkIndicator(false);
            }
        }

        if(isContaminated) // CONTAMINATE -- variables and methods at bottom of class
            UpdateContaminate();
    }

    public void SetValues(int hp, float cd)
    {
        health = hp;
        currentHealth = health;
        invulnTime = cd;
    }

    public bool TakeDamage(int damageAmount, Vector2 damageSpot, float knockForce)
    {
        if (canTakeDamage)
        {
            //canTakeDamage = false;
            //StartCoroutine(Invuln());
            // --- removed temporarily cus it messed up shotgun
            // - may not be necessary,
            // -  unless melee attacks are hitting the same enemy too many times (melee.collisionInterval)

            if(marked)
            {
                damageAmount *= 2;
                marked = false;
                SetMarkIndicator(false);
            }

            currentHealth -= damageAmount;
            PopUpDamageText T = Instantiate(damageText, damageSpot, Quaternion.identity);
            T.SendMessage("SetTextRun", damageAmount);

            if (thisEnemy != null && knockForce > 0)
            {
                thisEnemy.GotKnocked();
                thisEnemy.Knockback(knockForce);
            }
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

    public void Mark(float dur)
    {
        marked = true;
        SetMarkIndicator(true);
        markDuration = dur;
        markTimer = 0;
    }

    private void SetMarkIndicator(bool b)
    {
        if (markedIndicator != null)
            markedIndicator.SetActive(b);
    }

    IEnumerator Invuln()
    {
        yield return new WaitForSeconds(invulnTime);
        if (!canTakeDamage)
        {
            canTakeDamage = true;
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
        if(mySpawner != null)
            mySpawner.CheckEnemiesAlive();
        //Destroy(this.gameObject);
    }

    public void ResetHealth()
    {
        isContaminated = false;
        currentHealth = health;
    }

    // NOT BEING USED
    public bool CheckOneShot(int damageAmount)
    {
        if (currentHealth == health && currentHealth - damageAmount < 0)
        {
            return true;
        }
        else
            return false;
    }

    public DropType GetDropType()
    {
        return thisDropType;
    }

    //   ~~~~~~~~~ Seraph Afflications ~~~~~~~~~~
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
            if(particles == null)
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
                TakeDamage(contaminateDamage, this.transform.position, 0);
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
