using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    [SerializeField] private PlayerState currentState;
    protected static PlayerStateManager this_SM;
    protected static MovementController MoveControl;
    protected static ShotController ShotControl;
    protected static AltShotController AltControl;

    public PlayerState Ready;
    public PlayerState Shooting;
    public PlayerState AltFiring;
    public PlayerState Reloading;
    public PlayerState Special;
    public PlayerState Damaged;

    bool isActive;

    void Start()
    {
        this_SM = this;
        MoveControl = this.GetComponent<MovementController>();
        ShotControl = this.GetComponent<ShotController>();
        AltControl = this.GetComponent<AltShotController>();

        // Define States
        Ready = new PlayerState_Ready(this_SM, "Ready");
        Shooting = new PlayerState_Shooting(this_SM, "Shooting", ShotControl.shotDuration);
        AltFiring = new PlayerState_AltFire(this_SM, "AltFire");
        Special = new PlayerState_Special(this_SM, "Special", MoveControl.specialDuration);
        Reloading = new PlayerState_Reloading(this_SM, "Reloading", ShotControl.reloadDuration);

        ChangeState(Ready);
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive)
            currentState.DoState();
    }

    public void ChangeState(PlayerState newState)
    {
        if (currentState != null)
            currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public PlayerState GetCurrentState()
    {
        return currentState;
    }
    public string GetStateName()
    {
        return currentState.GetName();
    }
    public void BackToReady()
    {
        ChangeState(Ready);
    }
    public void ActivePlayer(bool b)
    {
        isActive = b;
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
