using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContaminateSeraph : Seraph
{
    public int damagePerTick;
    public float duration;
    public float damageInterval;

    // Start is called before the first frame update
    void Start()
    {
        switch (myBlood)
        {
            case BloodType.A:   // Increased Damage per tick
                damagePerTick += 1; // = (int)(damagePerTick * 1.5f); 
                break;
            case BloodType.B:   // Increased Duration
                duration *= 1.5f;
                break;
            case BloodType.AB:  // Increased Damage and Duration
                damagePerTick += 1; // (int)(damagePerTick * 1.5f);
                duration *= 1.5f;
                break;
            case BloodType.O:   // Increased Frequency
                damageInterval /= 1.75f;
                break;
        }
    }
    public override void StartEffect(ShootableEntity entity, Vector2 hitPoint)
    {
        entity.Contaminate(damagePerTick, duration, damageInterval);
    }

    public override void DoEffect()
    {
    }

    public override void EndEffect()
    {
    }
}
