using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Seraph : MonoBehaviour
{
    //public enum Genome { None, Rupture, Siphon, Contaminate, Surge, Calcify }
    //public Genome seraphType;

    public string MyName;

    public enum BloodType { A, B, AB, O }
    public BloodType myBlood;

    private void Awake()
    {
        int roll = Random.Range(0, 102);

        if (roll < 31)
            myBlood = BloodType.A; // 30% chance
        else if (roll < 61)
            myBlood = BloodType.B; // 30% chance
        else if (roll < 76)
            myBlood = BloodType.AB;// 15% chance
        else
            myBlood = BloodType.O; // 25% chance
    }

    private Vector2 targetPos;

    public abstract void StartEffect(Entity entity, Vector2 hitPoint);
    public abstract void DoEffect();
    public abstract void EndEffect();

    public virtual void SetTargetPos(Vector2 pos)
    {
        targetPos = pos;
    }

    public virtual Seraph GetThisSeraph()
    {
        return this;
    }
}
