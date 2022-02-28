using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    [SerializeField] private PlayerState currentState;
    protected static PlayerStateManager this_SM;
    protected static MovementController MoveControl;
    //protected static ShotController ShotControl;
    //protected static AltShotController AltControl;
    //protected static EquipmentManager EquipControl;
    //protected static MeleeController MeleeControl;
    //protected static WeaponManager WeaponControl;

    protected static Player this_Player;

    public PlayerState Ready;
    public PlayerState Shooting;
    public PlayerState Dashing;
    public PlayerState AltFiring;
    public PlayerState Reloading;
    public PlayerState Rechamber;
    public PlayerState Special;
    public PlayerState Damaged;

    public PlayerState MeleeAttack;
    public PlayerState Attack1;
    public PlayerState Attack2;
    public PlayerState Attack3;
    public PlayerState AttackRecover;

    public PlayerState ShootMain;
    public PlayerState RechamberMain;
    public PlayerState ReloadMain;
    public PlayerState AttackMain;
    public PlayerState RecoverMain;
    public PlayerState SpecMain;

    public PlayerState ShootAlt;
    public PlayerState RechamberAlt;
    public PlayerState ReloadAlt;
    public PlayerState AttackAlt;
    public PlayerState RecoverAlt;
    public PlayerState SpecAlt;

    public PlayerState FullReload;

    bool isActive;

    void Start()
    {
        this_SM = this;
        this_Player = this.GetComponent<Player>();
        MoveControl = this.GetComponent<MovementController>();
        //ShotControl = this.GetComponent<ShotController>();
        //AltControl = this.GetComponent<AltShotController>();
        //EquipControl = FindObjectOfType<EquipmentManager>();
        //WeaponControl = FindObjectOfType<WeaponManager>();
        //MeleeControl = this.GetComponent<MeleeController>();

        // Define States
        Ready = new PlayerState_Ready(this_SM, "Ready");
        //Shooting = new PlayerState_Shooting(this_SM, "Shooting", ShotControl.shotDuration);
        Dashing = new PlayerState_Dash(this_SM, "Dash", MoveControl.dashDuration);
        Damaged = new PlayerState_Damaged(this_SM, this_Player, "Damaged", this_Player.invulnDuration);
        //AltFiring = new PlayerState_AltFire(this_SM, "AltFire");
        Special = new PlayerState_Special(this_SM, "Special");
        Reloading = new PlayerState_Reloading(this_SM, "Reloading", true);
        Rechamber = new PlayerState_Rechamber(this_SM, "Rechamber", true);

        MeleeAttack = new PlayerState_MeleeAttack(this_SM, "MeleeAtk");
        //Attack1 = new PlayerState_MeleeAttack(this_SM, "Attack1", MeleeControl.attackDuration_1, MeleeControl.preAttackDelay_1, 1);
        //Attack2 = new PlayerState_MeleeAttack(this_SM, "Attack2", MeleeControl.attackDuration_2, MeleeControl.preAttackDelay_2, 2);
        //Attack3 = new PlayerState_MeleeAttack(this_SM, "Attack3", MeleeControl.attackDuration_3, MeleeControl.preAttackDelay_3, 3);
        //AttackRecover = new PlayerState_MeleeRecover(this_SM, "AtkRecover", MeleeControl.recoverTime, 0, 1);

        ShootMain = new PlayerState_RangedFire(this_SM, "Shoot-Main", true);
        RechamberMain = new PlayerState_Rechamber(this_SM, "Rechamber-Main", true);
        ReloadMain = new PlayerState_Reloading(this_SM, "Reload-Main", true);
        AttackMain = new PlayerState_MeleeFire(this_SM, "Attack-Main", true);
        RecoverMain = new PlayerState_MeleeRecover(this_SM, "Recover-Main", true);
        SpecMain = new PlayerState_SpecialFire(this_SM, "Spec-Main", true);

        ShootAlt = new PlayerState_RangedFire(this_SM, "Shoot-Alt", false);
        RechamberAlt = new PlayerState_Rechamber(this_SM, "Rechamber-Alt", false);
        ReloadAlt = new PlayerState_Reloading(this_SM, "Reload-Alt", false);
        AttackAlt = new PlayerState_MeleeFire(this_SM, "Attack-Alt", false);
        RecoverAlt = new PlayerState_MeleeRecover(this_SM, "Recover-Alt", false);
        SpecAlt = new PlayerState_SpecialFire(this_SM, "Spec-Alt", false);

        FullReload = new PlayerState_Reloading(this_SM, "FullReload", true);

        ChangeState(Ready);
    }

    /*public bool IsMelee()
    {
        return WeaponControl.IsMelee();
    }*/

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
