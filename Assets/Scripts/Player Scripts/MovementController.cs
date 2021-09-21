using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public static MovementController _moveControl;
    public static MovementController GetMoveController { get { return _moveControl; } }

    public EquipmentManager.SpecialType currentSpecial;

    public enum Movement { Normal, Boost, AltHold, Roll}
    public Movement currentMoveType;

    [Header("Debug Tools")]
    public bool RightClickToMoveDEACT;

    //[Header ("Components")]
    private Rigidbody2D rb;

    private AudioManager audioManager;

    [Header("Variable Stats")]
    public float baseMoveSpeed;
    public float maxMoveSpeed;
    public float slowMoveSpeed_alt;
    [SerializeField] private float currentMoveSpeed = 1.25f;

    [Header("Boost")]
    public float boostAmount = 1.5f;
    public float killShotBoostMultiplier = 1.5f;
    public float boostDuration = 3f;

    [Header("Special")]
    public float specialDuration;
    public float specialCooldown;
    public float specialSpeed;

    // Private variables
    [SerializeField] private float targetSpeed;
    private float neutralSpeed;
    private float boostLerpTimer;
    private float recoilTimer;
    private float recoilDuration;

    private Vector2 direction;

    private void Awake()
    {
        currentSpecial = EquipmentManager.SpecialType.None;
        _moveControl = this;
    }

    private void Start()
    {
        audioManager = AudioManager.GetAudioManager;

        rb = this.GetComponent<Rigidbody2D>();
        currentMoveSpeed = baseMoveSpeed;
    }

    // **FIXED UPDATE**
    private void FixedUpdate()
    {
        float moveH = Input.GetAxis("Horizontal") * currentMoveSpeed;
        float moveV = Input.GetAxis("Vertical") * currentMoveSpeed;

        if (recoilTimer < recoilDuration)
        {
            recoilTimer += Time.fixedDeltaTime;
        }
        else
        {
            rb.velocity = new Vector2(moveH, moveV);
        }
        direction = new Vector2(moveH, moveV);

        //  SET ANIMATION
        //FindObjectOfType<PlayerAnimation>().SetDirection(direction);

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
    public void Recoil(bool moveDir, Vector2 dir, float force, float duration)
    {
        if(moveDir)
            rb.AddForce(direction * force);
        else
            rb.AddForce(dir * force);
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
}
