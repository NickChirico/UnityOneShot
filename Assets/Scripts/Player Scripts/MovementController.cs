using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public static MovementController _moveControl;
    public static MovementController GetMoveController { get { return _moveControl; } }

    private static PlayerInputActions playerInputActions;
    public static PlayerInputActions GetInputActions { get { return playerInputActions; } }

    public EquipmentManager.SpecialType currentSpecial;

    public enum Movement { Normal, Boost, AltHold, Roll, Hold, Slow }
    public Movement currentMoveType;

    [Header("Debug Tools")]
    public bool RightClickToMoveDEACT;

    //[Header ("Components")]
    private Rigidbody2D rb;

    private AudioManager audioManager;
    private AnimationController animControl;

    [Header("Variable Stats")]
    public float baseMoveSpeed;
    public float maxMoveSpeed;
    public float slowMoveSpeed_alt;
    [SerializeField] private float currentMoveSpeed = 1.25f;

    [Header("Boost")]
    public float boostAmount = 1.5f;
    public float killShotBoostMultiplier = 1.5f;
    public float boostDuration = 3f;

    [Header("Dash")]
    public float dashForce;
    public float dashDuration;
    public float dashCooldown;
    private bool canDash = true;


    [Header("Special")]
    public float specialDuration;
    public float specialCooldown;
    public float specialSpeed;

    [Header("Melee Attributes")]
    [Range(1, 10)] public float delayStopTime;

    // Private variables
    private float targetSpeed;
    private float neutralSpeed;
    private float boostLerpTimer;
    private float recoilTimer;
    private float recoilDuration;
    private float thrustTimer;
    private float thrustDuration;
    private float dashTimer;

    private Vector2 direction;

    private void Awake()
    {
        currentSpecial = EquipmentManager.SpecialType.None;
        _moveControl = this;

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    /*private void Movement_performed(InputAction.CallbackContext context)
    {
        //Debug.Log("INPUT");
        Vector2 inputVector = context.ReadValue<Vector2>();
        float speed = 500f;
        rb.AddForce(inputVector * speed, ForceMode2D.Force);
    }*/

    private void Start()
    {
        audioManager = AudioManager.GetAudioManager;
        animControl = AnimationController.GetAnimController;

        rb = this.GetComponent<Rigidbody2D>();
        currentMoveSpeed = baseMoveSpeed;
    }
    public Vector2 GetDirection()
    { return direction; }

    // **FIXED UPDATE**
    private void FixedUpdate()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        if (recoilTimer < recoilDuration)
        {
            recoilTimer += Time.fixedDeltaTime;
        }
        else if (thrustTimer < thrustDuration)
        {
            thrustTimer += Time.deltaTime;
        }
        else if (dashTimer < dashDuration)
        {
            dashTimer += Time.deltaTime;
        }
        else
        {
            rb.velocity = inputVector *= currentMoveSpeed;
        }

        direction = inputVector;

        //  SET ANIMATION
        //FindObjectOfType<PlayerAnimation>().SetDirection(direction);
        //Debug.Log(currentMoveSpeed);

        if (Mathf.Abs(rb.velocity.x) > 0.4f || Mathf.Abs(rb.velocity.y) > 0.4f)
        {
            if (!animControl.GetWalk())
                animControl.SetWalk(true);
        }
        else
        {
            if (animControl.GetWalk())
                animControl.SetWalk(false);
        }

        animControl.SetFlipX(direction);


        //  Movement INDICATOR ?
        //Vector2 indicatiorLoc = new Vector2(this.transform.position.x + (moveH * 0.15f), this.transform.position.y + (moveV * 0.15f));
        //destinationIndicator.transform.position = indicatiorLoc;
    }

    // **UPDATE**
    private void Update()
    {
        float t = Time.deltaTime;
        switch (currentMoveType.ToString())
        {
            case "Boost":
                boostLerpTimer += Time.deltaTime;
                if (boostLerpTimer > boostDuration)
                    boostLerpTimer = boostDuration;

                t = boostLerpTimer / boostDuration;
                t = t * t * t * (t * (6f * t - 15f) + 10f);
                neutralSpeed = baseMoveSpeed;
                break;
            case "AltHold":
                t = Time.deltaTime * 4;
                neutralSpeed = slowMoveSpeed_alt;
                break;
            case "Roll":
                //
                break;
            case "Hold":
                t = Time.deltaTime * delayStopTime;
                neutralSpeed = 0;
                break;
            case "Slow":
                t = Time.deltaTime * delayStopTime;
                neutralSpeed = baseMoveSpeed * 0.33f;
                break;
            default:
                // Normal 
                neutralSpeed = baseMoveSpeed;
                t = Time.deltaTime * 8;
                break;
        }
        targetSpeed = Mathf.Lerp(targetSpeed, neutralSpeed, t);
        if (Mathf.Abs(targetSpeed - baseMoveSpeed) < 0.01f)
        { currentMoveType = Movement.Normal; }

        currentMoveSpeed = targetSpeed;
        // Always returning to neutralSpeed from targetSpeed
        //Update ends with changing currentMoveSpeed --> rb move in 
    }

    public void SetMoveType(Movement type)
    {
        currentMoveType = type;
    }

    public void UpdateCurrentEquipment(EquipmentManager.SpecialType boost)
    {
        currentSpecial = boost;
    }

    // ~~ Recoil ~~
    public void Recoil(bool isDashMOVEdir, Vector2 dir, float force, float duration)
    {
        if (isDashMOVEdir)
            rb.AddForce(direction * force, ForceMode2D.Impulse);
        else
            rb.AddForce(dir * force, ForceMode2D.Impulse);
        recoilTimer = 0;
        recoilDuration = duration;
    }

    // ~~ Alt Fire (Hold) ~~
    public void SetAltHold(bool isHold)
    {
        if (isHold)
            SetMoveType(Movement.AltHold);
        else
            SetMoveType(Movement.Normal);
    }

    // ~~ SPEED BOOST ~~
    public void SpeedBoost(bool isKillShot)//BoostType)
    {
        SetMoveType(Movement.Boost);

        if (isKillShot)
        {
            targetSpeed = baseMoveSpeed * (boostAmount * killShotBoostMultiplier);
        }
        else
        {
            targetSpeed = baseMoveSpeed * boostAmount;
        }

        if (targetSpeed > maxMoveSpeed)
        {
            targetSpeed = maxMoveSpeed;
        }
        boostLerpTimer = 0f;
        audioManager.PlayBoostSound(isKillShot);
    }

    // ~~ Thrust ~~
    public void Thrust(Vector2 dir, float force, float dur)
    {
        rb.AddForce(dir * force, ForceMode2D.Impulse);
        thrustTimer = 0;
        thrustDuration = dur;
    }
    public void Hold()
    {
        SetMoveType(Movement.Hold);
    }
    public void MoveNormal()
    {
        SetMoveType(Movement.Normal);
    }

    // ~~ DASH ~~
    public void Dash()
    {

        rb.velocity = Vector2.zero;
        rb.AddForce(direction * dashForce, ForceMode2D.Impulse);
        audioManager.PlayDashSound();
        dashTimer = 0;
        StartCoroutine(DashCooldown());

    }
    IEnumerator DashCooldown()
    {
        canDash = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    public bool CanDash()
    {
        return canDash;
    }
}
