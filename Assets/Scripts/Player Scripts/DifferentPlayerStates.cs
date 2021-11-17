using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifferentPlayerStates : MonoBehaviour
{
}

/* READY
 *  Can Shoot or use any ability from Ready 
 *      check for input ==> Shooting
 *      check for input ==> Alt Fire
 *      ?? check for input ==> Special ??
 *  No Duration;
 */
public class PlayerState_Ready : PlayerState
{
    public override void DoState()
    {
        // READY : read inputs
        float firePressed = InputAction.Player.Fire.ReadValue<float>(); // ~~~~~~~ InputAction ~~~ WORKS ~~~~~
        float reloadPressed = InputAction.Player.Reload.ReadValue<float>();
        float specPressed = InputAction.Player.Fire2.ReadValue<float>();
        float dashPressed = InputAction.Player.Dash.ReadValue<float>();

        if (firePressed > 0)
        {
            if (SM.IsMelee()) // If (IsMelee)
            {
                if(Melee.CanAttack())
                    SM.ChangeState(SM.MeleeAttack); // Go to ATTACKING State
            }
            else if (Shot.HasShot())
                SM.ChangeState(SM.Shooting); //  Go to SHOOTING State
        }
        if (specPressed > 0 && Spec.CanSpecial())
        {
            SM.ChangeState(SM.Special);
        }
        if (reloadPressed > 0 && Shot.currentAmmo < Shot.ammoCapacity)
        {
            SM.ChangeState(SM.Reloading);
        }
        if (dashPressed > 0 && Move.CanDash())
        {
            SM.ChangeState(SM.Dashing);
        }
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public PlayerState_Ready(PlayerStateManager myManager, string myName) :
        base(myManager, myName)
    {
    }
}

/* SHOOTING
 *  Shooting the main Shot 
 *  last ShotDuration, then ==> Ready
 */
public class PlayerState_Shooting : PlayerState
{
    public override void DoState()
    {
        /*if (Input.GetButton("Fire1"))
        {
            // wait
        }
        else if (Input.GetButtonUp("Fire1")) // MUST FIX WHEN CHARGE
        {
            //Shot.ButtonRelease();
        }
        else
        {*/

        if (timer < Duration)
        {
            timer += Time.deltaTime;
        }
        else
        {
            if (Shot.currentAmmo > 0)
            {
                if (Shot.doRechamber)
                    SM.ChangeState(SM.Rechamber);
                else
                {
                    Shot.SetHasShot(true);
                    SM.BackToReady();
                }
            }
            else
                SM.ChangeState(SM.Reloading);
        }
        //}
    }

    public override void Enter()
    {
        base.Enter();
        timer = 0;
        //Shot.ButtonPress(); // Commence Shot
        Shot.CommenceShot();
        Duration = Shot.delayBetweenShots;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public PlayerState_Shooting(PlayerStateManager myManager, string myName, float myDur) :
        base(myManager, myName, myDur)
    {
    }
}

/* ALT FIRE
 *  Using Alt Fire 
 *  last altDuration, then ==> Ready
 */
public class PlayerState_AltFire : PlayerState
{
    public override void DoState()
    {
        float altFirePressed = InputAction.Player.Fire2.ReadValue<float>();

        if (altFirePressed > 0)
        {
            if (Alt.CanAltFire())
                Alt.CommenceAltFire();
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            Alt.ToggleHoldingAlt(false);
            Shot.AltFireAction();
        }
        else
        {
            timer += Time.deltaTime;
            if (timer > Duration) // Alt.PutAwayTime;
            {
                if (Shot.HasShot())
                    SM.BackToReady();
                else
                    SM.ChangeState(SM.Rechamber);
            }
        }
    }

    public override void Enter()
    {
        base.Enter();
        Duration = Alt.GetPutAwayTime();
        Alt.ToggleHoldingAlt(true);
        Alt.AltMovement();
        Move.SetAltHold(true);
    }

    public override void Exit()
    {
        base.Exit();
        Move.SetAltHold(false);
    }

    public PlayerState_AltFire(PlayerStateManager myManager, string myName) :
        base(myManager, myName)
    {
    }
}


//// ~~~~~~~~~~~~~~~~
//// ~~~ SPECIAL ~~~~ --> completely remove AltControl
//// ~~~~~~~~~~~~~~~~
public class PlayerState_Special : PlayerState
{
    float t1;
    float atimer;

    bool didShoot = false;
    bool doArc = false;
    int meleeC = 0;
    bool canThrust = true;
    public override void DoState()
    {
        float specialPressed = InputAction.Player.Fire2.ReadValue<float>();

        if (doArc)
        {
            if (specialPressed == 0) // shoot early
            {
                if (Spec.CanSpecial())
                    ShootSpecial();
            }
            else
            {
                if (t1 < 1)
                {
                    t1 += Time.deltaTime;
                    float lerpRatio = t1;
                    //Vector2 offset = Spec.aimCurve_arc.Evaluate(lerpRatio) * new Vector2(1,0);
                    Spec.AimProjectileArc(lerpRatio);//, offset);   
                }
                else // shoot after time
                {
                    if (Spec.CanSpecial())
                        ShootSpecial();
                }
            }
        }
        else if (!didShoot)
        {
            ShootSpecial(); // sets --> didShoot = true
        }


        if (didShoot)
        {
            if (meleeC > 0)
            {                
                timer += Time.deltaTime;



                if (timer > Duration)
                {
                    SM.ChangeState(SM.AttackRecover); // to RECOVER
                }
                else if (timer > PrepDuration)
                {
                    atimer += Time.deltaTime;
                    if (canThrust)
                    {
                        Melee.AttackThrust(meleeC);
                        canThrust = false;
                    }
                }
                if (atimer > 0.1f)
                {
                    atimer = 0;
                    Melee.Attack(meleeC);
                }


            }
            else
            { // perform "other ranged" special
                if (timer < Duration)
                { timer += Time.deltaTime; }
                else
                {
                    if (Shot.currentAmmo > 0)
                        SM.BackToReady();
                    else
                        SM.ChangeState(SM.Reloading);
                }
            }


            /*if (timer > PrepDuration)
            {
                if (!didShoot)
                    ShootSpecial();
            }*/
        }
        

    }

    private void ShootSpecial()
    {
        didShoot = true;
        Spec.CommenceSpecial(Spec.GetCurSpecial());
        
        if(doArc)
            Spec.InitArcAim(false);
    }

    public override void Enter()
    {
        base.Enter();
        //Move.SetMoveType(MovementController.Movement.Slow);

        if (Spec.GetCurSpecial().Equals(SpecialController.Special.Lunge))
            meleeC = 3; // Knife attackInterval - Lunge attack [3] == 4th entry
        else if (Spec.GetCurSpecial().Equals(SpecialController.Special.GreatSlam))
            meleeC = 2;
        else
            meleeC = 0;
        atimer = 0.1f;
        canThrust = true;

        doArc = (Spec.GetCurSpecial().Equals(SpecialController.Special.Mortar));
        didShoot = false;
        timer = 0;
        t1 = 0;
        Duration = Spec.sp_Duration;
        PrepDuration = Spec.sp_PreDelay;

        //
        if(doArc)
            Spec.InitArcAim(true);
        //
    }

    public override void Exit()
    {
        base.Exit();
        Move.SetMoveType(MovementController.Movement.Normal);
        Spec.EndNimble();
    }

    public PlayerState_Special(PlayerStateManager myManager, string myName) :
        base(myManager, myName)
    {
    }
}

/* RELOADING
 *  Delay until Ready after Shot
 *  last reloadDuration, then ==> Ready
 *  can Alt Fire from reload, but still have to reload ==> AltFire
 */
public class PlayerState_Reloading : PlayerState
{
    bool soundPlayed;
    public override void DoState()
    {
        float dashPressed = InputAction.Player.Dash.ReadValue<float>();
        float fire2Pressed = InputAction.Player.Dash.ReadValue<float>();

        if (dashPressed > 0 && Move.CanDash())
        {
            SM.ChangeState(SM.Dashing);
        }
        else if (fire2Pressed > 0 && Spec.CanSpecial())
        {
            SM.ChangeState(SM.Special);
        }
        else if (timer < Duration)
        {
            timer += Time.deltaTime;
            if (timer > Duration * 0.66f && !soundPlayed)
            {
                ReloadSuccess();
                //Shot.PlayReloadSound();
                soundPlayed = true;
            }
        }
        else if (timer > Duration)
        {
            SM.BackToReady();
        }
    }

    public override void Enter()
    {
        base.Enter();
        soundPlayed = false;
        Duration = Shot.reloadDuration;
        Shot.PlayFullReloadSound();

        Shot.ToggleAimLineColor(true);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void ReloadSuccess()
    {
        Shot.currentAmmo = Shot.ammoCapacity;
        Shot.ToggleAimLineColor(false);
        Shot.SetHasShot(true);
        Shot.UpdateAmmoUI();
    }

    public PlayerState_Reloading(PlayerStateManager myManager, string myName, float myDur) :
        base(myManager, myName, myDur)
    {
    }
}

public class PlayerState_Rechamber : PlayerState
{
    bool soundPlayed;
    public override void DoState()
    {
        float altFirePressed = InputAction.Player.Fire2.ReadValue<float>();
        float dashPressed = InputAction.Player.Dash.ReadValue<float>();

        if (altFirePressed > 0 && Alt.CanAltFire())
        {
            SM.ChangeState(SM.AltFiring);
        }
        if (dashPressed > 0 && Move.CanDash())
        {
            SM.ChangeState(SM.Dashing);
        }
        else if (timer < Duration)
        {
            timer += Time.deltaTime;
            if (timer > Duration * 0.40f && !soundPlayed)
            {
                Shot.SetHasShot(true);
                Shot.ToggleAimLineColor(false);

                Shot.PlayReloadSound();
                soundPlayed = true;
            }
        }
        else
        {
            SM.BackToReady();
        }
    }

    public override void Enter()
    {
        base.Enter();
        soundPlayed = false;
        Duration = Shot.rechamberDuration;

        Shot.ToggleAimLineColor(true);

        if (Shot.currentAmmo < 1)
            SM.ChangeState(SM.Reloading);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public PlayerState_Rechamber(PlayerStateManager myManager, string myName, float myDur) :
        base(myManager, myName, myDur)
    {
    }
}

// ~~~~~~~~ MELEE ~~~~~~~~
public class PlayerState_MeleeAttack : PlayerState
{
    float atimer = 0;
    bool canThrust;
    public override void DoState()
    {
        timer += Time.deltaTime;

        if (timer > Duration)
        {
            SM.ChangeState(SM.AttackRecover); // to RECOVER
        }
        else if (timer > PrepDuration)
        {
            atimer += Time.deltaTime;
            if (canThrust)
            {
                Melee.AttackThrust(AttackInterval);
                canThrust = false;
            }
        }

        if (atimer > Melee.collisionInterval)
        {
            atimer = 0;
            Melee.Attack(AttackInterval);
        }
    }

    public override void Enter()
    {
        base.Enter();
        AttackInterval = Melee.GetCurrentInterval();
        Duration = Melee.attackDurArr[AttackInterval];
        PrepDuration = Melee.preDelayArr[AttackInterval];
        // COLLISION INTERVAL?

        // if(!canMoveWhileAttacking)
        Move.Hold();
        canThrust = true;
        Melee.CanAim(false);
        atimer = Melee.collisionInterval;
        Melee.PrepAttack(AttackInterval); // GIVE PARAMETER (1, 2, 3) for PREP(), THRUST(), and ATTACK()
    }

    public override void Exit()
    {
        Melee.CanAim(true);
        Move.MoveNormal();
        base.Exit();
    }

    public PlayerState_MeleeAttack(PlayerStateManager myManager, string myName) :
        base(myManager, myName)
    {
    }
}

public class PlayerState_MeleeRecover : PlayerState
{
    public override void DoState()
    {
        float firePressed = InputAction.Player.Fire.ReadValue<float>();
        float fire2Pressed = InputAction.Player.Fire2.ReadValue<float>();
        float dashPressed = InputAction.Player.Dash.ReadValue<float>();

        timer += Time.deltaTime;

        if (timer > Duration)
        {
            Melee.ResetAttackSequence();
            SM.BackToReady();
        }
        else if (firePressed > 0)
        {
            if (SM.IsMelee() && Melee.CanAttack() && AttackInterval < Melee.intervalCount)
            {
                /*switch(Melee.intervalCount)
                {
                    case 1:
                        SM.ChangeState(SM.Attack2);
                        break;
                    case 2:
                        SM.ChangeState(SM.Attack3);
                        break;
                    default:
                        break;
                }*/
                SM.ChangeState(SM.MeleeAttack);
            }
        }
        else if (fire2Pressed > 0 && Spec.CanSpecial())
        {
            SM.ChangeState(SM.Special);
        }
        else if (dashPressed > 0 && Move.CanDash())
        {
            SM.ChangeState(SM.Dashing);
        }
    }

    public override void Enter()
    {
        base.Enter();
        Duration = Melee.recoverTime;
        AttackInterval = Melee.GetCurrentInterval();
        Melee.Recover();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public PlayerState_MeleeRecover(PlayerStateManager myManager, string myName, float myDur, float myPrep, int myInterval) :
        base(myManager, myName, myDur, myPrep, myInterval)
    {
    }
}

public class PlayerState_Dash : PlayerState
{
    public override void DoState()
    {
        timer += Time.deltaTime;

        if (timer > Duration)
        {
            if (SM.IsMelee() || (!SM.IsMelee() && Shot.HasShot()))
                SM.BackToReady();
            else
                SM.ChangeState(SM.Reloading);
        }
    }

    public override void Enter()
    {
        base.Enter();
        Move.Dash();

        if (SM.IsMelee())
            Melee.ResetAttackSequence();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public PlayerState_Dash(PlayerStateManager myManager, string myName, float myDur) :
        base(myManager, myName, myDur)
    {
    }
}

public class PlayerState_Damaged : PlayerState
{
    public override void DoState()
    {
        timer += Time.deltaTime;

        if (timer > Duration)
        {
            if (SM.IsMelee() || (!SM.IsMelee() && Shot.HasShot()))
                SM.BackToReady();
            else
                SM.ChangeState(SM.Reloading);
        }
    }

    public override void Enter()
    {
        base.Enter();
        Vector2 knockDir = new Vector2(Random.Range(-360, 360), Random.Range(-360, 360)).normalized;
        Move.Recoil(false, knockDir, player.knockedForce, player.invulnDuration);

        if (SM.IsMelee())
            Melee.ResetAttackSequence();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public PlayerState_Damaged(PlayerStateManager myManager, Player p, string myName, float myDur) :
        base(myManager, p, myName, myDur)
    {
    }
}