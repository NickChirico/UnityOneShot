using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breacher : Enemy
{
    [Header("BREACHER")]
    public Weapon enemyWeapon;
    private RangedWeapon myShotgun;
    public LineRenderer aimLine;
    public Vector2 shootOffset;

    Vector2 shotLoc;
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
        shotLoc = new Vector2(this.transform.position.x + shootOffset[0], this.transform.position.y + shootOffset[1]);

        aimLine.enabled = true;
        attackDir = dir;
        aimLine.SetPosition(0, shotLoc);
        aimLine.SetPosition(1, GetRayOrigin() + (attackDir * attackRange));

        if (myAnim != null && isAlive)
        {
            myAnim.SetTrigger("Attack");
        }
    }

    public override void Attack(Vector2 dir)
    {
        //base.Attack(dir);
        canAttack = false;
        StartCoroutine(AttackCooldown());

        myShotgun.Fire(shotLoc, attackDir);
        aimLine.enabled = false;
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
