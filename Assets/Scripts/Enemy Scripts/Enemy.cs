using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] public string enemyName;
    [SerializeField] private float currentMovespeed;
    private int currentHealth;
    [SerializeField] private int maxHealth;

    private Transform playerLoc;

    public SpriteRenderer sp;
    public Rigidbody2D rb;

    //[Header("Information")]
    private Vector2 rayOrigin;
    private Vector2 direction;
    private float distanceToPlayer;
    //public Ray2D rayToPlayer;

    [Header("Components")]
    public LineRenderer lineRend;
    public bool EnableLineRend;


    [Header("Variables")]
    public float visionRange;
    public bool playerSpotted;
    [Header("Idle")]
    public float idleDuration;
    [Header("Alert")]
    public float alertDuration;
    [Header("Patrol")]
    public float patrolRange;
    public float patrolDuration;
    public float patrolSpeed;
    [Header("Chase")]
    public float chaseDuration;
    public float chaseSpeed;
    [Header("Attack")]
    public int attackDamage;
    public float attackRange;
    public float attackDuration;
    public float attackDelay;
    public float attackLungeForce;
    public bool canAttack;

    private void Awake()
    {
        sp = this.GetComponent<SpriteRenderer>();
        rb = this.GetComponent<Rigidbody2D>();
        playerLoc = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        if (EnableLineRend)
        {
            lineRend.gameObject.SetActive(true);
            lineRend.enabled = true;
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;

        Introduction();
    }

    private void Introduction()
    {
        Debug.Log(enemyName + ", " + playerLoc);
    }

    // UPDATE
    private void Update()
    {

        UpdateInformation();

    }

    public void UpdateInformation()
    {
        rayOrigin = new Vector2(this.transform.position.x, this.transform.position.y);
        direction = (playerLoc.position - this.transform.position).normalized;
        distanceToPlayer = Vector2.Distance(playerLoc.position, this.transform.position);

        lineRend.SetPosition(0, rayOrigin);
        lineRend.SetPosition(1, rayOrigin + direction * visionRange);
    }

    // GETTERS/SETTERS
    public Vector2 GetDirection()
    {
        return direction;
    }
    public float GetDistance()
    {
        return distanceToPlayer;
    }

    // PUBLIC METHODS
    public bool IsInLineOfSight()
    {
        //rayToPlayer = new Ray2D(rayOrigin, direction * visionRange);
        RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, direction, visionRange);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Terrain"))
                {
                    return false;
                }
                else if (hit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool IsInAttackRange()
    {
        if (canAttack)
        {
            if (GetDistance() <= attackRange)
                return true;
            else
                return false;
        }
        else
            return false;

        /* Not shooting a raycast, because enemies should still perform the attack
         * even if they dont have LineOfSight
         */
    }

    // ********************
    // * Override Methods *
    // ********************
    public abstract void SetUp();

    public virtual void Patrol(Vector2 dest)
    {
        // PATROL LOGIC base

        // move to that position
        Vector2 dir = dest - rayOrigin;
        float moveH = dir.x * patrolSpeed;
        float moveV = dir.y * patrolSpeed;
        rb.velocity = new Vector2(moveH, moveV);
    }

    public virtual Vector2 GetPatrolPoint()
    {
        // Get random positon to patrol to nearby
        Vector2 randomDir = new Vector2(Random.value, Random.value).normalized;
        float randomDistance = Random.Range(patrolRange * 0.25f, patrolRange);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, randomDir, randomDistance);
        if (hit.collider != null)
        {
            //if (hit.collider.CompareTag("Terrain"))
            //    return hit.point;
            return hit.point;
        }
        else
        {
            return rayOrigin + (randomDir * randomDistance);
        }
    }

    public virtual void Chase()
    {
        // CHASE LOGIC base

        // Assuming has LineOfSight,
        // Move in direction of player
        float moveH = direction.x * chaseSpeed;
        float moveV = direction.y * chaseSpeed;
        rb.velocity = new Vector2(moveH, moveV);
    }

    public virtual void Attack(Vector2 dir)
    {
        // ATTACK LOGIC base
        canAttack = false;
        rb.AddForce(dir * attackLungeForce);

    }

    public virtual void Dodge()
    {
        // DODGE LOGIC base
        Debug.Log("Dodging...");

    }
}
