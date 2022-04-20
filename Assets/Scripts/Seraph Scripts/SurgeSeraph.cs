using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurgeSeraph : Seraph
{
    public float boostAmount;
    public float boostDuration;
    public float echoFrequency;

    // Start is called before the first frame update
    void Start()
    {
        switch (myBlood)
        {
            case BloodType.A:
                // Gives great flat move speed boost
                boostAmount *= 2.25f;
                echoFrequency = 0.05f;
                break;
            case BloodType.B:
                // Give small boost for longer time
                boostAmount *= 1.75f;
                boostDuration *= 2.5f;
                echoFrequency = 0.09f;
                break;
            case BloodType.AB:
                boostAmount *= 2f;
                boostDuration *= 1.75f;
                echoFrequency = 0.075f;
                // Medium boost, medium duration

                break;
            case BloodType.O:
                boostAmount *= 1.65f;
                echoFrequency = 0.075f;
                // While reloading, increased speed. 

                break;
        }
    }
    public override void StartEffect(Entity entity, Vector2 hitPoint)
    {
        if (entity == null || entity.IsEnemy()) // if an enemy was hit, boost PLAYER
        {
            if (myBlood != BloodType.O)
            {
                MovementController move = MovementController.GetMoveController;
                move.SeraphBoost(boostAmount, boostDuration, echoFrequency);
            }
        }
    }
    public override void DoEffect()
    {
        if (myBlood.Equals(BloodType.O))
        {
            MovementController move = MovementController.GetMoveController;
            move.SetSpeed(false, boostAmount);
        }
    }

    public override void EndEffect()
    {
        if (myBlood.Equals(BloodType.O))
        {
            MovementController move = MovementController.GetMoveController;
            move.SetSpeed(true, 1);
        }
    }
}
