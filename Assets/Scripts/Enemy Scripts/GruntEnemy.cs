using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntEnemy : Enemy
{
    [Header("GRUNT")]
    public float gruntSpeed;

    public override void SetUp()
    {
        
    }

    public override void Die()
    {
        if (enemyName == "Boss")
        {
            Enemy[] enemies = FindObjectsOfType<Baneling>();
            foreach (Enemy e in enemies)
                Destroy(e.gameObject);
        }

        base.Die();
    }
}
