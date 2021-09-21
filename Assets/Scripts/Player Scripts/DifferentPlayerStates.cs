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
        if (Input.GetButton("Fire1") && Shot.HasShot())
        {
            SM.ChangeState(SM.Shooting); //  Go to SHOOTING State
        }
        if (Input.GetButton("Fire2") && Alt.CanAltFire())
        {
            SM.ChangeState(SM.AltFiring);
        }
        /*if (Input.GetButtonDown("Fire1"))
        {
            Shot.ShootGetButtonDown();
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            Shot.ShootGetButtonUp();
        }*/
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
        else if (Input.GetButtonUp("Fire1"))
        {
            Shot.ButtonRelease();
        }
        else
        {
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
        if (Input.GetButton("Fire2"))
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
                    SM.ChangeState(SM.Reloading);
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
        if (Input.GetButton("Fire2") && Alt.CanAltFire())
        {
            SM.ChangeState(SM.AltFiring);
        }
        else if (timer < Duration)
        {
            timer += Time.deltaTime;
            if (timer > Duration * 0.60f && !soundPlayed)
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
        Duration = Shot.reloadDuration;

        Shot.ToggleAimLineColor(true);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public PlayerState_Reloading(PlayerStateManager myManager, string myName, float myDur) :
        base(myManager, myName, myDur)
    {
    }
}