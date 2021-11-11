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

    private void Start()
    {
        canTakeDamage = true;
        currentHealth = health; // temp for 123 spawn in enemies
        try 
        { 
            thisEnemy = this.GetComponent<Enemy>();
        }
        catch
        {
            thisEnemy = null;
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
    }

    public void SetValues(int hp, float cd)
    {
        health = hp;
        currentHealth = health;
        invulnTime = cd;
    }

    public bool TakeDamage(int damageAmount, Vector2 damageSpot)
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

            if (thisEnemy != null)
            {
                thisEnemy.GotHit();
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
        mySpawner.CheckEnemiesAlive();
        //Destroy(this.gameObject);
    }

    public void ResetHealth()
    {
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
}
