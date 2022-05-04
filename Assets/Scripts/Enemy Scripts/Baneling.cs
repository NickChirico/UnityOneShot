using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baneling : Enemy
{
    [Header("Baneling")]

    public int blastDamage;
    public float blastPostureDamage;
    public float blastRadius;
    public float blastKnockForce;
    public float blastDuration;
    public float blastDelay;


    public Explosion blast_prefab;

    public Animator Anim_baneling;
    public override void SetUp()
    {
        chaseSpeed += Random.Range(-0.15f, 0.15f);
    }

    public override void SetChaseAnim(bool b)
    {
        Anim_baneling.SetBool("isRoll", b);
    }
    
    public override void Attack(Vector2 dir)
    {
        canAttack = false;


        StartCoroutine(Detonate());
    }

    IEnumerator Detonate()
    {
        Anim_baneling.SetBool("isRoll", false);
        yield return new WaitForSeconds(blastDelay);
        GameObject.Find("AudioManager").GetComponent<AudioManager>().ExplosionSource.PlayOneShot(GameObject.Find("AudioManager").GetComponent<AudioManager>().explosionSound);
        Explosion E = Instantiate(blast_prefab, this.transform.position, Quaternion.identity);
        E.SetValues(blastDamage, blastRadius, blastKnockForce, blastDuration);
        Die();
        //Destroy(this.gameObject);

    }
}
