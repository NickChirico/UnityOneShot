using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [Header("RIFLEMAN")]
    public Weapon enemyWeapon;
    private RangedWeapon myRifle;
    public LineRenderer aimLine;

    public override void SetUp()
    {
        if (enemyWeapon != null)
        {
            myRifle = (RangedWeapon)Instantiate(enemyWeapon, this.transform);
            //attackRange = myRifle.range;
            attackDamage = myRifle.shotDamage;
        }
    }

    Vector2 attackDir;
    public override void Aim(Vector2 dir)
    {
        aimLine.enabled = true;
        attackDir = dir;
        aimLine.SetPosition(0, this.transform.position);
        aimLine.SetPosition(1, GetRayOrigin() + attackDir * attackRange);
    }

    public override void Attack(Vector2 dir)
    {
        //base.Attack(dir);
        canAttack = false;

        myRifle.Fire(this.transform.position, attackDir);
        aimLine.enabled = false;
    }
}
