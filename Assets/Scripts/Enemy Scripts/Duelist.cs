using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duelist : Enemy
{
    [Header("DUELIST")]
    public Animator animDuel;
    public MeleeWeapon knife;
    public float height;
    public AudioClip attackClip2, attackClip3;

    public GameObject SeraphDrop;
    [Range(0f, 1f)] public float dropChance;

    public override void SetUp()
    {

    }


    public override void Attack(Vector2 dir)
    {
        canAttack = false;

        switch (Random.Range(0, 4))
        {
            case 1:
                StartCoroutine(ComboAttack());
                break;
            case 2:
                StartCoroutine(LungeAttack());
                break;
            default:
                StartCoroutine(QuickAttack());
                break;
        }
    }

    public override void Die()
    {

        base.Die();
    }

    private IEnumerator QuickAttack()
    {
        animDuel.SetTrigger("Prep");
        knife.Fire(GetRayOrigin(height), GetDirection());
        knife.PrepAttack(0);
        yield return new WaitForSeconds(attackDelay);
        animDuel.SetTrigger("Attack1");
        //mySource.clip = attackClip;
        //mySource.Play();
        Thrust(attackLungeForce);
        knife.Attack(0);
        yield return new WaitForSeconds(attackDuration);
        knife.SetIndicator(false);
        ResetAttack(attackCooldown);
        animDuel.SetTrigger("End");
    }

    private IEnumerator ComboAttack()
    {
        animDuel.SetTrigger("Prep");
        knife.Fire(GetRayOrigin(height), GetDirection());
        knife.PrepAttack(0);
        yield return new WaitForSeconds(attackDelay);
        animDuel.SetTrigger("Attack3");
        //mySource.clip = attackClip;
        //mySource.Play();
        Thrust(attackLungeForce);
        knife.Attack(0);

        yield return new WaitForSeconds(0.15f);
        knife.Fire(GetRayOrigin(height), GetDirection());
        knife.PrepAttack(2);
        yield return new WaitForSeconds(attackDelay);
        //mySource.clip = attackClip2;
        //mySource.Play();
        Thrust(attackLungeForce);
        knife.Attack(2);

        yield return new WaitForSeconds(0.15f);
        knife.Fire(GetRayOrigin(height), GetDirection());
        knife.PrepAttack(3);
        yield return new WaitForSeconds(attackDelay);
        //mySource.clip = attackClip3;
        //mySource.Play();
        Thrust(attackLungeForce);
        knife.Attack(3);

        yield return new WaitForSeconds(attackDuration);
        knife.SetIndicator(false);
        ResetAttack(attackCooldown + 0.5f);
        animDuel.SetTrigger("End");
    }

    private IEnumerator LungeAttack()
    {
        animDuel.SetTrigger("Prep");
        knife.Fire(GetRayOrigin(height), GetDirection());
        knife.PrepAttack(0);
        yield return new WaitForSeconds(attackDuration);
        animDuel.SetTrigger("Attack1");
        //mySource.clip = attackClip;
        //mySource.Play();
        Thrust(attackLungeForce * 4);
        knife.Attack(3);
        knife.SetIndicator(false);
        ResetAttack(attackCooldown - 0.5f);
        animDuel.SetTrigger("End");
    }

    private void Thrust(float force)
    {
        rb.AddForce(GetDirection() * force, ForceMode2D.Impulse);
    }
}
