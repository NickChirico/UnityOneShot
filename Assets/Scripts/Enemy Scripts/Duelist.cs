using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duelist : Enemy
{
    public override void SetUp()
    {
        throw new System.NotImplementedException();
    }


    public override void Attack(Vector2 dir)
    {
        canAttack = false;

        switch (Random.Range(0, 3))
        {
            case 1:
                Debug.Log("CASE 1");
                break;
            case 2:
                Debug.Log("CASE 2");
                break;
            default:
                Debug.Log("CASE default");
                break;
        }
    }
}
