using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiphonSeraph : Seraph
{
    public HealingOrb healthOrb;

    public int healAmount;

    void Start()
    {

    }

    void Update()
    {

    }
    public override void StartEffect(ShootableEntity entity, Vector2 hitPoint)
    {
        HealingOrb H = Instantiate(healthOrb, hitPoint, Quaternion.identity);
        H.SetHealAmount(healAmount);
    }
    public override void DoEffect()
    {
    }

    public override void EndEffect()
    {
    }



}
