using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
    public static MeleeController _meleeControl;
    public static MeleeController GetMeleeControl { get { return _meleeControl; } }

    private PlayerInputActions playerInputActions;

    [Header("Components")]
    private MovementController moveController;
    private AudioManager audioManager;

    [Header("GAME OPTIONS")]
    public bool usingMouse;

    [Header("Variable Stats")]
    public LayerMask hittableEntity;
    public GameObject tempAttackDisplay;
    public float collisionInterval;
    public float recoverTime;
    public float meleeCooldown;

    [Header("Attack 1")]
    public int attack_1_Damage;
    public float attackDuration_1;
    public float swingRange_1;
    public float attackRadius_1;
    public float thrustForce_1;
    public float thrustDuration_1;
    public float preAttackDelay_1;

    [Header("Attack 2")]
    public int attack_2_Damage;
    public float attackDuration_2;
    public float swingRange_2;
    public float attackRadius_2;
    public float thrustForce_2;
    public float thrustDuration_2;
    public float preAttackDelay_2;

    [Header("Attack 3")]
    public int attack_3_Damage;
    public float attackDuration_3;
    public float swingRange_3;
    public float attackRadius_3;
    public float thrustForce_3;
    public float thrustDuration_3;
    public float preAttackDelay_3;

    // Private Variables
    private Vector3 direction, rayOrigin, attackPoint;
    private bool canAim = true;
    private bool canAttack = true;

    [Header("NEW VARS")]
    public int intervalCount = 0;
    public int currentInterval;
    public int[] damageArr;
    public float[] attackDurArr;
    public float[] swingRangeArr;
    public float[] attackRadiusArr;
    public float[] thrustForceArr;
    public float[] thrustDurArr;
    public float[] preDelayArr;

    private void Awake()
    {
        _meleeControl = this;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }
    void Start()
    {
        moveController = MovementController.GetMoveController;
        audioManager = AudioManager.GetAudioManager;

        //attackDuration1 = thrustDuration_1 + preAttackDelay_1;
        //attackDuration2 = thrustDuration_2 + preAttackDelay_2;
        //attackDuration3 = thrustDuration_3 + preAttackDelay_3;
    }
    public void SetWeaponStats(MeleeWeapon weap)
    {
        intervalCount = weap.intervalCount;
        currentInterval = 0;
        damageArr = weap.damageArr;
        attackDurArr = weap.attackDurArr;
        swingRangeArr = weap.swingRangeArr;
        attackRadiusArr = weap.attackRadiusArr;
        thrustForceArr = weap.thrustForceArr;
        thrustDurArr = weap.thrustDurArr;
        preDelayArr = weap.preDelayArr;
        recoverTime = weap.recoverTime;
        meleeCooldown = weap.comboCooldown;
    }

    public void SetIndicator(bool b)
    {
        tempAttackDisplay.GetComponent<SpriteRenderer>().enabled = b;
    }

    private void Update()
    {
        if (canAim)
            UpdateDirection();
        UpdateAttackPoint();    
    }

    public void CanAim(bool b)
    { 
        canAim = b;
        if (b)
        {
            tempAttackDisplay.GetComponent<SpriteRenderer>().color = Color.black;
            tempAttackDisplay.transform.localScale = Vector3.one * 0.25f;
        }
    }
    public bool CanAttack()
    { return canAttack; }

    public int GetCurrentInterval()
    { return currentInterval; }

    public Vector3 GetDirection()
    {
        return direction;
    }
    public Vector3 GetRayOrigin()
    {
        return rayOrigin;
    }

    private void UpdateDirection()
    {
        if (usingMouse)
        {
            // Mouse Look Controls
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z * -1)); // invert cam Z to make 0
            direction = (targetPos - this.transform.position).normalized;
        }
        else
        {
            // Controller Controls
            Vector2 aimVector = playerInputActions.Player.Aim.ReadValue<Vector2>();
            Vector2 moveVector = playerInputActions.Player.Move.ReadValue<Vector2>();

            if (aimVector.Equals(Vector2.zero))
            {
                if (moveVector.Equals(Vector2.zero))
                    aimVector = Vector2.up;
                else
                    aimVector = moveVector;
            }

            direction = aimVector;
        }
    }
    private void UpdateAttackPoint()
    {
        rayOrigin = new Vector3(this.transform.position.x,
            this.transform.position.y + 0.25f,
            this.transform.position.z);

        float swingRange = swingRangeArr[currentInterval];

        attackPoint = rayOrigin + (direction * swingRange);

        tempAttackDisplay.transform.position = attackPoint;
    }

    public void AttackThrust(int interval)
    {
        float dur = thrustDurArr[interval];
        float force = thrustForceArr[interval];

        if (dur != 0 && force != 0)
        {
            moveController.Thrust(direction, force, dur);
        }

        // Play sound based on attack interval
        audioManager.PlayMeleeSound(interval);

        /*switch (interval)
        {
            case 1:
                moveController.Thrust(direction, thrustForce_1, thrustDuration_1);
                break;
            case 2:
                moveController.Thrust(direction, thrustForce_2, thrustDuration_2);
                break;
            case 3:
                moveController.Thrust(direction, thrustForce_3, thrustDuration_3);
                break;
            default:
                break;
        }

        // Play sound based on attack interval
        audioManager.PlayMeleeSound(interval);*/

    }

    public void PrepAttack(int interval)
    {
        tempAttackDisplay.GetComponent<SpriteRenderer>().color = Color.yellow;

        float rad = attackRadiusArr[interval];
        tempAttackDisplay.transform.localScale = Vector3.one * rad * 1.25f;
    }
    public void Attack(int interval)
    {
        // TEMP DISPLAY STUFF
        tempAttackDisplay.GetComponent<SpriteRenderer>().color = Color.red;
        Collider2D[] hitEnemies;

        float rad = attackRadiusArr[interval];
        int damageToPass = damageArr[interval];

        tempAttackDisplay.transform.localScale = Vector3.one * (rad * 2); //diameter - TEMP display
        hitEnemies = Physics2D.OverlapCircleAll(attackPoint, rad, hittableEntity);

        if (hitEnemies != null)
        {
            foreach (Collider2D hit in hitEnemies)
            {
                if (hit.CompareTag("Terrain"))
                { }
                else if(hit.CompareTag("Enemy"))
                {
                    ShootableEntity entity = hit.GetComponent<ShootableEntity>();
                    if (entity != null)
                    {
                        ApplyAttack(entity, hit.transform.position, damageToPass);
                    }
                }
            }
        }
    }

    public void Recover()
    {
        tempAttackDisplay.GetComponent<SpriteRenderer>().color = Color.green;
        tempAttackDisplay.transform.localScale = Vector3.one * 0.25f;
        if (currentInterval < intervalCount - 1)
            currentInterval++;
        else
            ResetAttackSequence();
    }

    public void ResetAttackSequence()
    {
        currentInterval = 0;
        tempAttackDisplay.GetComponent<SpriteRenderer>().color = Color.black;
        tempAttackDisplay.transform.localScale = Vector3.one * 0.25f;

        canAttack = false;
        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(meleeCooldown);
        canAttack = true;
    }
    private void ApplyAttack(ShootableEntity entityHit, Vector2 hitPoint, int damage)
    {
        int damageToDeal = damage;
        // switch( CURRENT ATTACK EFFECT??)
        // { different augments? }
        // ...CalculateProximityDamage(damage, hitDistance)


        entityHit.TakeDamage(damageToDeal, hitPoint);

    }

    private void OnDrawGizmos()
    {
        if(attackPoint!= null)
            Gizmos.DrawWireSphere(attackPoint, attackRadius_1);
    }
}
