using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuptureSeraph : Seraph
{
    public Explosion Explosion_Prefab;
    public DestroyDelay preIndicator;
    //public Vector2 targetPos;

    public int ruptureDamage;
    public float ruptureRadius;
    public float ruptureDuration;
    public float ruptureKnockback;
    public float ruptureOnsetDelay;
    void Start()
    {
        switch (myBlood)
        {
            case BloodType.A:
                ruptureDamage = (int)(ruptureDamage * 1.5f); // 50% Increased Damage
                break;
            case BloodType.B:
                ruptureKnockback *= 1.5f;
                break;
            case BloodType.AB:
                ruptureDamage = (int)(ruptureDamage * 1.5f); // 50% Increased Damage
                ruptureKnockback *= 1.5f;
                break;
            case BloodType.O:
                // 2nd Explosion -- in SpawnExplosion()
                break;
        }
    }

    public override void StartEffect(ShootableEntity entity, Vector2 hitPoint)
    {
        DestroyDelay P = Instantiate(preIndicator, hitPoint, Quaternion.identity);
        P.SetDelay(ruptureOnsetDelay);

        StartCoroutine(SpawnExplosion(hitPoint));

    }
    public override void DoEffect()
    {
    }

    public override void EndEffect()
    {
    }

    IEnumerator SpawnExplosion(Vector2 hitPoint)
    {
        yield return new WaitForSeconds(ruptureOnsetDelay);
        Explosion E = Instantiate(Explosion_Prefab, hitPoint, Quaternion.identity);
        E.SetValues(ruptureDamage, ruptureRadius, ruptureKnockback, ruptureDuration);

        if (myBlood.Equals(BloodType.O))
        {
            yield return new WaitForSeconds(ruptureOnsetDelay*2);
            Explosion E2 = Instantiate(Explosion_Prefab, hitPoint, Quaternion.identity);
            E2.SetValues(ruptureDamage, ruptureRadius/1.5f, ruptureKnockback, ruptureDuration);
        }
    }
}
