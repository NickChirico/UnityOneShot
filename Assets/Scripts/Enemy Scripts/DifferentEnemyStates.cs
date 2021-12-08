using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifferentEnemyStates : MonoBehaviour
{

}

/* IDLE
 * If in VisionRange,   ==> State: Alert
 * Else if duration expires and nothing in range,   ==> State: Patrol 
 * 
 */
public class EnemyState_Idle : EnemyState
{ 
    public override void DoState()
    {
        if (timer < Duration)
        {
            timer += Time.deltaTime;

            if (thisEnemy.IsInLineOfSight())
            {
                SM.ChangeState(SM.Alert);
            }
        }
        else
        {
            SM.ChangeState(SM.Patrol);
        }
    }

    public override void Enter()
    {
        base.Enter();
        thisEnemy.sp.color = Color.white;

    }

    public override void Exit()
    {
        base.Exit();
    }

    public EnemyState_Idle(EnemyStateManager myManager, Enemy thisEnemy, string myName, float myDur) :
        base(myManager, thisEnemy, myName, myDur)
    {

    }
}

/* ALERT
 * Determine Action:
 *  If within AttackRange && CanAttack,     ==> Attack
 *  Else if not AttackRange but still in VisionRange ==> Chase
 *  Else -> out of VisionRange      ==> Idle
 */
public class EnemyState_Alert : EnemyState
{
    int action;
    public override void DoState()
    {
        if (timer < Duration)
        {
            timer += Time.deltaTime;
            // AlertAction();
        }
        else
        {
            // Determine action
            // Change state to action state

            switch (action)
            {
                case 1:
                    SM.ChangeState(SM.Attack);
                    break;

                case 2:
                    SM.ChangeState(SM.Chase);
                    break;

                default:
                    SM.ChangeState(SM.Idle);
                    break;
            }
        }
    }

    private int DetermineAction()
    {
        if (thisEnemy.IsInAttackRange())
            return 1; // ATTACK
        else if (thisEnemy.IsInLineOfSight())
            return 2; // CHASE
        else
            return 0; // IDLE
    }

    public override void Enter()
    {
        base.Enter();
        thisEnemy.sp.color = Color.yellow;

        action = DetermineAction();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public EnemyState_Alert(EnemyStateManager myManager, Enemy thisEnemy, string myName, float myDur) :
        base(myManager, thisEnemy, myName, myDur)
    {

    }
}
/* CHASE
 *  If within AttackRange && CanAttack,     ==> Attack
 *      After attacking ==> Alert
 * 
 */
public class EnemyState_Chase : EnemyState
{
    public override void DoState()
    {
        if (timer < Duration)
        {
            if (thisEnemy.IsInAttackRange())
                SM.ChangeState(SM.Attack); // ==> Attack
            else if (thisEnemy.IsInLineOfSight())
            {
                timer += Time.deltaTime;
                thisEnemy.Chase();
            }
            else
            {
                SM.ChangeState(SM.Alert); // ==> Alert
            }
        }
        else
        {
            SM.ChangeState(SM.Alert);   // ==> Alert
        }
    }

    public override void Enter()
    {
        base.Enter();
        thisEnemy.sp.color = Color.green;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public EnemyState_Chase(EnemyStateManager myManager, Enemy thisEnemy, string myName, float myDur, float mySpeed) :
        base(myManager, thisEnemy, myName, myDur, mySpeed)
    {

    }
}

/* PATROL
 *  If not in VisionRange, Patrol back and forth randomly nearby
 *  If in VisionRange,      ==> Alert
 *  Else        ==> Idle
 */
public class EnemyState_Patrol : EnemyState
{
    Vector2 dest;
    public override void DoState()
    {
        if (timer < Duration)
        {
            if (thisEnemy.IsInLineOfSight())
                SM.ChangeState(SM.Alert); // ==> Alert
            else
            {
                timer += Time.deltaTime;
                thisEnemy.Patrol(dest); // Enemy.Patrol
            }
        }
        else
        {
            SM.ChangeState(SM.Idle); // ==> Idle
        }
    }

    public override void Enter()
    {
        base.Enter();
        Duration = Random.Range(thisEnemy.idleDuration*0.5f, thisEnemy.idleDuration*1.5f);

        thisEnemy.sp.color = Color.cyan;

        dest = thisEnemy.GetPatrolPoint();
        //Debug.Log(dest);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public EnemyState_Patrol(EnemyStateManager myManager, Enemy thisEnemy, string myName, float myDur, float mySpeed) :
        base(myManager, thisEnemy, myName, myDur, mySpeed)
    {

    }
}

/* ATTACK
 *  After attacking,    ==> Alert
 *  
 */
public class EnemyState_Attack : EnemyState
{
    float delay;
    Vector2 attackDir;
    public override void DoState()
    {
        if (timer < Duration)
        { 
            if (timer < delay)
            {
                // should be an animation event, so we dont need this
            }
            else
            { 
                if(thisEnemy.canAttack)
                    thisEnemy.Attack(attackDir);    
            }
            timer += Time.deltaTime;
        }
        else
        {
            SM.ChangeState(SM.Alert);
            // animation event
        }
    }

    public override void Enter()
    {
        base.Enter();
        delay = thisEnemy.attackDelay;
        attackDir = thisEnemy.GetDirection();
        thisEnemy.sp.color = Color.red;

        thisEnemy.Aim(attackDir);
    }

    public override void Exit()
    {
        base.Exit();
        thisEnemy.canAttack = true;
    }

    public EnemyState_Attack(EnemyStateManager myManager, Enemy thisEnemy, string myName, float myDur) :
        base(myManager, thisEnemy, myName, myDur)
    {

    }
}

/* DODGE
 *  If Enemy(CanDodge) -- called from Enemy/SpecificEnemy
 *      Dodge to a direction, set CanDodge = false;
 *      After Dodging,  
 *          If CanAttack    ==> Attack
            Else            ==> Alert
 */
public class EnemyState_Dodge : EnemyState
{
    public override void DoState()
    {
        if (timer < Duration)
        {
            timer += Time.deltaTime;
        }
        else
        {
            SM.ChangeState(SM.Idle);
        }
    }

    public override void Enter()
    {
        base.Enter();
        thisEnemy.sp.color = Color.blue;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public EnemyState_Dodge(EnemyStateManager myManager, Enemy thisEnemy, string myName, float myDur, float mySpeed) :
        base(myManager, thisEnemy, myName, myDur, mySpeed)
    {

    }

}

// KNOCKED BACK
public class EnemyState_Knocked : EnemyState
{
    public override void DoState()
    {
        if (timer < Duration)
        {
            timer += Time.deltaTime;
        }
        else
        {
            SM.ChangeState(SM.Alert);
        }
    }

    public override void Enter()
    {
        base.Enter();
        thisEnemy.sp.color = Color.black;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public EnemyState_Knocked(EnemyStateManager myManager, Enemy thisEnemy, string myName, float myDur) :
        base(myManager, thisEnemy, myName, myDur)
    {

    }

}
