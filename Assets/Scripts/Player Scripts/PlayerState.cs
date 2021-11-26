﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected PlayerStateManager SM;
    protected Player player;
    protected PlayerController playerControl;

    protected MovementController Move;
    protected ShotController Shot;
    protected AltShotController Alt;
    protected SpecialController Spec;
    protected MeleeController Melee;

    protected PlayerInputActions InputAction;

    protected bool isMainWeap;
    protected string Name;
    protected float Duration;
    protected float PrepDuration;
    protected int AttackInterval;

    protected float timer;

    public PlayerState(PlayerStateManager manager, string name)
    {
        InputAction = MovementController.GetInputActions;
        Move = MovementController.GetMoveController;
        Shot = ShotController.GetShotControl;
        Alt = AltShotController.GetAltControl;
        Melee = MeleeController.GetMeleeControl;
        Spec = SpecialController.GetSpecialController;
        SM = manager;
        Name = name;
        playerControl = PlayerController.GetPlayerController;
    }
    public PlayerState(PlayerStateManager manager, string name, bool mainWeap)
    {
        InputAction = MovementController.GetInputActions;
        SM = manager;
        Name = name;
        Move = MovementController.GetMoveController;
        playerControl = PlayerController.GetPlayerController;
        isMainWeap = mainWeap;
    }

    public PlayerState(PlayerStateManager manager, string name, float dur)
    {
        InputAction = MovementController.GetInputActions;
        Move = MovementController.GetMoveController;
        Shot = ShotController.GetShotControl;
        Alt = AltShotController.GetAltControl;
        Melee = MeleeController.GetMeleeControl;
        Spec = SpecialController.GetSpecialController;
        SM = manager;
        Name = name;
        Duration = dur;
        playerControl = PlayerController.GetPlayerController;
    }
    public PlayerState(PlayerStateManager manager, Player p, string name, float dur)
    {
        InputAction = MovementController.GetInputActions;
        Move = MovementController.GetMoveController;
        Shot = ShotController.GetShotControl;
        Alt = AltShotController.GetAltControl;
        Melee = MeleeController.GetMeleeControl;
        Spec = SpecialController.GetSpecialController;
        SM = manager;
        player = p;
        Name = name;
        Duration = dur;
        playerControl = PlayerController.GetPlayerController;
    }

    public PlayerState(PlayerStateManager manager, string name, float dur, float prepDur, int interval)
    {
        InputAction = MovementController.GetInputActions;
        Move = MovementController.GetMoveController;
        Shot = ShotController.GetShotControl;
        Alt = AltShotController.GetAltControl;
        Melee = MeleeController.GetMeleeControl;
        Spec = SpecialController.GetSpecialController;
        SM = manager;
        Name = name;
        Duration = dur;
        PrepDuration = prepDur;
        AttackInterval = interval;
        playerControl = PlayerController.GetPlayerController;
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
