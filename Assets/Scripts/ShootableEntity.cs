using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableEntity : MonoBehaviour
{
    public int Health = 3;
    public int currentHealth;

    TempEnemy thisEnemy;

    private void Start()
    {
        currentHealth = Health;
        try
        {
            thisEnemy = this.GetComponent<TempEnemy>();
        }
        catch
        {
            Debug.Log("No Enemy Found");
        }
    }

    public bool TakeDamage(int damageAmount, Vector2 damageSpot)
    {
        if (thisEnemy != null)
        {
            thisEnemy.Shot(damageAmount, damageSpot);
        }

        currentHealth -= damageAmount;
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

    private void Die()
    {
        gameObject.SetActive(false);
    }

    // NOT BEING USED
    public bool CheckOneShot(int damageAmount)
    {
        if (currentHealth == Health && currentHealth - damageAmount < 0)
        {
            return true;
        }
        else
            return false;
    }
}
