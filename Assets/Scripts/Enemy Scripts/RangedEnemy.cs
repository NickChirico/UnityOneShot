using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [Header("RIFLEMAN")]
    public Weapon enemyWeapon;
    private RangedWeapon myRifle;
    public LineRenderer aimLine;
    public Vector2 shootOffset;

    Vector2 shotLoc;
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
        shotLoc = new Vector2(this.transform.position.x + shootOffset[0], this.transform.position.y + shootOffset[1]);

        aimLine.enabled = true;
        attackDir = dir;
        aimLine.SetPosition(0, shotLoc);
        aimLine.SetPosition(1, GetRayOrigin() + (attackDir * attackRange));
    }

    public override void Attack(Vector2 dir)
    {
        //base.Attack(dir);
        canAttack = false;
        StartCoroutine(AttackCooldown());

        myRifle.Fire(shotLoc, attackDir);
        
        aimLine.enabled = false;
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
