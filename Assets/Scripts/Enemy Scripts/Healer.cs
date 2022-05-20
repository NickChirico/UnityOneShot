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

        switch (Random.Range(0, 2))
        {
            case 1:
                StartCoroutine(ShootOne());
                break;
            default:
                StartCoroutine(ShootBurst());
                break;
        }
    }

    private IEnumerator ShootOne()
    {
        if(myAnim != null)
        { myAnim.SetTrigger("Attack"); }
        seek_player proj = Instantiate(Projectile, shootPoint.transform.position, Quaternion.identity);
        proj.SetMyEnemy(this);
        yield return new WaitForSeconds(attackDuration);
        canAttack = true;
    }

    private IEnumerator ShootBurst()
    {
        if (myAnim != null)
        { myAnim.SetTrigger("Attack"); }
        seek_player proj = Instantiate(Projectile, shootPoint.transform.position, Quaternion.identity);
        proj.SetMyEnemy(this);
        yield return new WaitForSeconds(0.33f);
        if (myAnim != null)
        { myAnim.SetTrigger("Attack"); }
        seek_player proj2 = Instantiate(Projectile, shootPoint.transform.position, Quaternion.identity);
        proj2.SetMyEnemy(this);
        yield return new WaitForSeconds(0.33f);
        if (myAnim != null)
        { myAnim.SetTrigger("Attack"); }
        seek_player proj3 = Instantiate(Projectile, shootPoint.transform.position, Quaternion.identity);
        proj3.SetMyEnemy(this);
        yield return new WaitForSeconds(attackDuration);
        canAttack = true;
    }

    public void Heal()
    {
        //float hp = 0.2f * MaxHealth;
        currentHealth += 12;
        if (healthBar != null) // HP BAR
            healthBar.SetHealth((float)currentHealth / (float)MaxHealth);
    }
    

}
