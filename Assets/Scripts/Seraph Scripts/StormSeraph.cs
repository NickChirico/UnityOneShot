using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormSeraph : Seraph
{
    //public Lightning bolt_prefab;

    public int boltDamage;
    public float chargedDuration;
    public float chargedReductionAmount;
    public GameObject LightningStrike;

    void Start()
    {
        switch (myBlood)
        {
            case BloodType.A:
                // Each hit reduces the charge build up - quicker detonation with fast attacks
                break;
            case BloodType.B:
                // higher bonus detonation damage if attacked in sweet spot 
                break;
            case BloodType.AB:
                // Shorter build up, and higher bonus detonation damage
                break;
            case BloodType.O:
                // Lightning strike briefly stuns
                break;
        }
    }

    public override void StartEffect(Entity entity, Vector2 hitPoint)
    {
        if (entity.IsCharged())
        {
            entity.AccelerateLightning(chargedReductionAmount);
        }
        else
        {
            entity.StormStrike(chargedDuration, boltDamage, LightningStrike);
        }
    }
    public override void DoEffect()
    {
    }

    public override void EndEffect()
    {
    }
}
