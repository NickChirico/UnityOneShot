using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected PlayerStateManager SM;
    protected MovementController Move;
    protected ShotController Shot;
    protected AltShotController Alt;

    protected string Name;
    protected float Duration;

    protected float timer;

    public PlayerState(PlayerStateManager manager, string name)
    {
        Move = MovementController.GetMoveController;
        Shot = ShotController.GetShotControl;
        Alt = AltShotController.GetAltControl;
        SM = manager;
        Name = name;
    }

    public PlayerState(PlayerStateManager manager, string name, float dur)
    {
        Move = MovementController.GetMoveController;
        Shot = ShotController.GetShotControl;
        Alt = AltShotController.GetAltControl;
        SM = manager;
        Name = name;
        Duration = dur;
    }

    // Update
    public abstract void DoState();

    // Enter/Exit
    public virtual void Enter()
    {
        timer = 0;
    }
    public virtual void Exit()
    {

    }

    // Getters/ Setters
    public string GetName()
    {
        return Name;
    }
    public float GetDuration()
    {
        return Duration;
    }




}
