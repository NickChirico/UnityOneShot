using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : Enemy
{
    [Header("HEALER")]

    public Animator animHealer;
    public seek_player Projectile;
    public GameObject shootPoint;

    public override void SetUp()
    {

    }

    public override void Attack(Vector2 dir)
    {
        canAttack = false;

        StartCoroutine(ShootOne());

        /*switch (Random.Range(0, 2))
        {
            case 1:
                //StartCoroutine(QuickAttack());
                break;
            default:
                //StartCoroutine(ComboAttack());
                break;
        }*/
    }

    private IEnumerator ShootOne()
    {
        seek_player proj = Instantiate(Projectile, shootPoint.transform.position, Quaternion.identity);


        yield return new WaitForSeconds(attackDuration);
        canAttack = true;
    }

    private IEnumerator Shoot()
    {

        yield return new WaitForSeconds(attackDuration);

    }

}
