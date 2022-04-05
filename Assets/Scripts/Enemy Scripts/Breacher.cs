using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breacher : Enemy
{
    [Header("BREACHER")]
    public Weapon enemyWeapon;
    private RangedWeapon myShotgun;
    public LineRenderer aimLine;
    public override void SetUp()
    {
        if (enemyWeapon != null)
        {
            myShotgun = (RangedWeapon)Instantiate(enemyWeapon, this.transform);
            //attackRange = myRifle.range;
            attackDamage = myShotgun.shotDamage;
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
        StartCoroutine(AttackCooldown());

        myShotgun.Fire(this.transform.position, attackDir);
        aimLine.enabled = false;
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
