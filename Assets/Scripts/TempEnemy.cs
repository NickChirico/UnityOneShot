using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEnemy : MonoBehaviour
{
    public PopUpDamageText damageText;

    ShootableEntity thisEntity;
    int health;

    void Start()
    {
        thisEntity = this.GetComponent<ShootableEntity>();
        health = thisEntity.currentHealth;
    }

    void Update()
    {
        
    }

    public void Shot(int damage, Vector2 damageSpot)
    {
        //Debug.Log("SHOT");

        PopUpDamageText T = Instantiate(damageText, damageSpot, Quaternion.identity);
        T.SendMessage("SetTextRun", damage);

    }
}
