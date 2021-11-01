using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    [SerializeField] private EnemyState currentState;
    protected static EnemyStateManager this_SM;
    protected static Enemy thisEnemy;
    //protected static Transform playerTransform;
    // protected static Player player;

    public EnemyState Idle;
    public EnemyState Alert;
    public EnemyState Chase;
    public EnemyState Patrol;
    public EnemyState Attack;
    public EnemyState Dodge;
    public EnemyState Knocked;

    private void Start()
    {
        this_SM = this;
        thisEnemy = this.GetComponent<Enemy>();
        //playerTransform = 

        Idle = new EnemyState_Idle(this_SM, thisEnemy, "Idle", thisEnemy.idleDuration);
        Alert = new EnemyState_Alert(this_SM, thisEnemy, "Alert", thisEnemy.alertDuration);
        Chase = new EnemyState_Chase(this_SM, thisEnemy, "Chase", thisEnemy.chaseDuration, thisEnemy.chaseSpeed);
        Patrol = new EnemyState_Patrol(this_SM, thisEnemy, "Patrol", thisEnemy.patrolDuration, thisEnemy.patrolSpeed);
        Attack = new EnemyState_Attack(this_SM, thisEnemy, "Attack", thisEnemy.attackDuration);
        Knocked = new EnemyState_Knocked(this_SM, thisEnemy, "Knocked", thisEnemy.invulnTime);


        ChangeState(Idle);
    }

    private void Update()
    {
        currentState.DoState();
    }

    public void ChangeState(EnemyState newState)
    {
        if (currentState != null)
            currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public EnemyState GetCurrentState()
    {
        return currentState;
    }
    public string GetStateName()
    {
        return currentState.GetName();
    }

    public void BackToIdle()
    {
        ChangeState(Idle);
    }

    /*public void PlayAnimation(AnimationState animationState)
    {
        if (currentAnimationState != animationState)
        {
            myBossAnimator.Play(animationState.ToString());
            currentAnimationState = animationState;
        }
    }*/





}


