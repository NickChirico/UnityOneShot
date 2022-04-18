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

    bool doStun = false;

    void Start()
    {
        switch (myBlood)
        {
            case BloodType.A:
                // Increased bolt damage
                boltDamage += 8;
                break;
            case BloodType.B:
                // Shorter build up
                chargedDuration -= 2.5f;
                break;
            case BloodType.AB:
                // Shorter build up, and higher bonus detonation damage
                boltDamage += 5;
                chargedDuration -= 1.25f;
                break;
            case BloodType.O:
                // Lightning strike briefly stuns
                doStun = true;
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
            entity.StormStrike(chargedDuration, boltDamage, doStun, LightningStrike);
        }
    }
    public override void DoEffect()
    {
    }

    public override void EndEffect()
    {
    }
}
