using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState
{
    protected EnemyStateManager SM;
    protected Enemy thisEnemy;
    protected string Name;
    protected float Duration;
    protected float MoveSpeed;

    protected float timer;

    // Overloaded Constructors
    public EnemyState(EnemyStateManager manager, Enemy enemy, string name, float dur)
    {
        SM = manager;
        thisEnemy = enemy;
        Name = name;
        Duration = dur;
    }
    public EnemyState(EnemyStateManager manager, Enemy enemy, string name, float dur, float speed)
    {
        SM = manager;
        thisEnemy = enemy;
        Name = name;
        Duration = dur;
        MoveSpeed = speed;



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

