using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Seraph : MonoBehaviour
{
    //public enum Genome { None, Rupture, Siphon, Contaminate, Surge, Calcify }
    //public Genome seraphType;

    public string Title;
    public string SubTitle;
    public string Description;
    public string[] VariantSubtitles;
    public string[] VariantDescriptions;


    public enum BloodType { A, B, AB, O }
    [Space(20)]
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

        switch (myBlood)
        {
            case BloodType.A:
                SubTitle = "Seraphim " + VariantSubtitles[0] + " "+ SubTitle;
                Description += VariantDescriptions[0];
                break;
            case BloodType.B:
                SubTitle = "Seraphim " + VariantSubtitles[1] + " " + SubTitle;
                Description += VariantDescriptions[1];
                break;
            case BloodType.AB:
                SubTitle = "Seraphim " + VariantSubtitles[2] + " " + SubTitle;
                Description += VariantDescriptions[2];
                break;
            case BloodType.O:
                SubTitle = "Seraphim " + VariantSubtitles[3] + " " + SubTitle;
                Description += VariantDescriptions[3];
                break;
        }
    }

    private Vector2 targetPos;

    public abstract void StartEffect(Entity entity, Vector2 hitPoint);
    public abstract void DoEffect(); // called in RELOAD
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
