using UnityEngine;

public abstract class Enemy : Entity
{
    [Header("ENEMY")]

    [SerializeField] public string enemyName;

    private Transform playerLoc;
    private EnemyStateManager SM;


    //[Header("Information")]
    private Vector2 rayOrigin;
    private Vector2 direction;
    private float distanceToPlayer;
    //public Ray2D rayToPlayer;

    [Header("Components")]
    public SpriteRenderer sp;
    public Rigidbody2D rb;
    public LineRenderer lineRend;
    public bool EnableLineRend;

    public EnemySpawner mySpawner;
    //ShootableEntity entity;


    [Header("Variables")]
    public int damageCollision;
    public int damageAttack;
    public float postureColl, postureAtk;
    public float visionRange;
    [HideInInspector] public bool playerSpotted;

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
    [Header("Knocked")]
    public float invulnTime;
    public float knockbackForce;



    private void Awake()
    {
        //sp = this.GetComponentInChildren<SpriteRenderer>();
        rb = this.GetComponent<Rigidbody2D>();
        SM = this.GetComponent<EnemyStateManager>();
        mySpawner = GameObject.Find("Enemy Spawner").GetComponent<EnemySpawner>();
        //entity = this.GetComponent<ShootableEntity>();
        playerLoc = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        if (EnableLineRend)
        {
            lineRend.gameObject.SetActive(true);
            lineRend.enabled = true;
        }
    }

    public override void Start()
    {
        base.Start();
        SetUp();
    }

    // UPDATE
    public override void Update()
    {
        base.Update();

        UpdateInformation();
    }

    public void CheckCollisionPlayer()
    {
        
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

    public Vector2 GetRayOrigin()
    {
        return rayOrigin;
    }
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
    public void GotKnocked()
    {
        SM.ChangeState(SM.Knocked);
    }

    public void Knockback(float force)
    {
        Vector2 knockBack = ((-1 * direction) * force);
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.AddForce(knockBack, ForceMode2D.Impulse);
    }

    // ********************
    // * Override Methods *
    // ********************

    public override bool TakeDamage(int damageAmount, Vector2 damageSpot, float knockForce, float postureDamage)
    {
        bool b = base.TakeDamage(damageAmount, damageSpot, knockForce, postureDamage);
        if (guardBroken)
        {
            GotKnocked();
            Knockback(knockForce);
        }
        return b;
    }

    public override void Die()
    {
        mySpawner.allEnemies.Remove(gameObject);
        base.Die();
    }

    public override void GuardBreak()
    {
        base.GuardBreak();

    }

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
        rb.AddForce(dir * attackLungeForce, ForceMode2D.Impulse);

    }

    public virtual void Dodge()
    {
        // DODGE LOGIC base
        Debug.Log("Dodging...");

    }

    public virtual void Aim(Vector2 dir)
    { }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            Player player = coll.gameObject.GetComponent<Player>();
            if (player.CanBeDamaged() && SM.GetCurrentState() != SM.Knocked) // PLayer can be damaged and enemy is not in "Knocked"
            {
                if (SM.GetCurrentState() == SM.Attack)
                    player.TakeDamage(this, damageAttack + Random.Range(0, 4), postureAtk);
                else
                    player.TakeDamage(this, damageCollision, postureColl);

            }
        }
    }

}
