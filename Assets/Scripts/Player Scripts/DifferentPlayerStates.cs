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
        //if (Input.GetButton("Fire1"))
        float firePressed = InputAction.Player.Fire.ReadValue<float>(); // ~~~~~~~ InputAction ~~~ WORKS ~~~~~
        float reloadPressed = InputAction.Player.Reload.ReadValue<float>();
        float altFirePressed = InputAction.Player.Fire2.ReadValue<float>();
        float dashPressed = InputAction.Player.Dash.ReadValue<float>();

        if (firePressed > 0)
        {
            if (SM.IsMelee())
            {
                if(Melee.CanAttack())
                    SM.ChangeState(SM.Attack1);
            }
            else if (Shot.HasShot())
                SM.ChangeState(SM.Shooting); //  Go to SHOOTING State
        }
        if (altFirePressed > 0 && Alt.CanAltFire())
        {
            SM.ChangeState(SM.AltFiring);
        }
        if (reloadPressed > 0 && Shot.currentAmmo < Shot.AmmoCapacity)
        {
            SM.ChangeState(SM.Reloading);
        }
        if (dashPressed > 0 && Move.CanDash())
        {
            SM.ChangeState(SM.Dashing);
        }
        if(!Shot.HasShot())
        {
            if (Shot.currentAmmo <= 1)
                SM.ChangeState(SM.Reloading);
            else
                SM.ChangeState(SM.Rechamber);
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
        if (Input.GetButton("Fire1"))
        {
            // wait
        }
        else if (Input.GetButtonUp("Fire1")) // MUST FIX WHEN CHARGE
        {
            Shot.ButtonRelease();
        }
        else
        {
            if (Shot.currentAmmo > 0)
                SM.ChangeState(SM.Rechamber);
            else
                SM.ChangeState(SM.Reloading);
        }
    }

    public override void Enter()
    {
        base.Enter();
        Shot.ButtonPress(); // Commence Shot
    }

    public override void Exit()
    {
        base.Exit();
        Shot.LoseAmmo();
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

public class PlayerState_Special : PlayerState
{
    public override void DoState()
    {
        if (Input.GetButton("Fire3"))
        {
            timer += Time.deltaTime;
            
        }
        else
        {
            timer += Time.deltaTime;
            
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

    public PlayerState_Special(PlayerStateManager myManager, string myName, float myDur) :
        base(myManager, myName, myDur)
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
        float altFirePressed = InputAction.Player.Fire2.ReadValue<float>();
        float dashPressed = InputAction.Player.Dash.ReadValue<float>();

        if (altFirePressed > 0 && Alt.CanAltFire())
        {
            SM.ChangeState(SM.AltFiring);
        }
        else if (dashPressed > 0 && Move.CanDash())
        {
            SM.ChangeState(SM.Dashing);
        }
        else if (timer < Duration)
        {
            timer += Time.deltaTime;
            if (timer > Duration * 0.75f && !soundPlayed)
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
        Shot.currentAmmo = Shot.AmmoCapacity;
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
        Move.Hold();
        canThrust = true;
        Melee.CanAim(false);
        Melee.PrepAttack(AttackInterval); // GIVE PARAMETER (1, 2, 3) for PREP(), THRUST(), and ATTACK()
    }

    public override void Exit()
    {
        Melee.CanAim(true);
        Move.MoveNormal();
        base.Exit();
    }

    public PlayerState_MeleeAttack(PlayerStateManager myManager, string myName, float myDur, float myPrep, int myInterval) :
        base(myManager, myName, myDur, myPrep, myInterval)
    {
    }
}

public class PlayerState_MeleeRecover : PlayerState
{
    public override void DoState()
    {
        float firePressed = InputAction.Player.Fire.ReadValue<float>();
        float dashPressed = InputAction.Player.Dash.ReadValue<float>();

        timer += Time.deltaTime;

        if (timer > Duration)
        {
            SM.BackToReady();
            Melee.ResetAttackSequence();
        }
        else if (firePressed > 0)
        {
            if (SM.IsMelee())
            {
                switch(Melee.intervalCount)
                {
                    case 1:
                        SM.ChangeState(SM.Attack2);
                        break;
                    case 2:
                        SM.ChangeState(SM.Attack3);
                        break;
                    default:
                        break;
                }
            }
        }
        else if (dashPressed > 0 && Move.CanDash())
        {
            SM.ChangeState(SM.Dashing);
        }
    }

    public override void Enter()
    {
        base.Enter();
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
            SM.BackToReady();
        }
    }

    public override void Enter()
    {
        base.Enter();
        Move.Dash();
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
            SM.BackToReady();
        }
    }

    public override void Enter()
    {
        base.Enter();
        Vector2 knockDir = new Vector2(Random.Range(-360, 360), Random.Range(-360, 360)).normalized;
        Move.Recoil(false, knockDir, player.knockedForce, player.invulnDuration);
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