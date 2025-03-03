using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float detectionRange = 5f;
    public float attackRange = 1f;
    public float attackCooldown = 2f;
    
    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    public float waitTime = 1f;
    
    [Header("Audio")]
    public AudioClip attackSound;
    public AudioClip deathSound;
    
    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    
    private Vector3 startPosition;
    private int currentPatrolIndex = 0;
    private float lastAttackTime;
    private float waitTimer;
    private bool isWaiting;
    private bool isDead = false;
    
    private enum EnemyState
    {
        Patrolling,
        Chasing,
        Attacking,
        Waiting
    }
    
    private EnemyState currentState = EnemyState.Patrolling;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        startPosition = transform.position;
        
        // Find player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }
    
    void Update()
    {
        if (isDead) return;
        
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        switch (currentState)
        {
            case EnemyState.Patrolling:
                Patrol();
                if (distanceToPlayer <= detectionRange)
                {
                    currentState = EnemyState.Chasing;
                }
                break;
                
            case EnemyState.Chasing:
                ChasePlayer();
                if (distanceToPlayer > detectionRange)
                {
                    currentState = EnemyState.Patrolling;
                }
                else if (distanceToPlayer <= attackRange)
                {
                    currentState = EnemyState.Attacking;
                }
                break;
                
            case EnemyState.Attacking:
                AttackPlayer();
                if (distanceToPlayer > attackRange)
                {
                    currentState = EnemyState.Chasing;
                }
                break;
                
            case EnemyState.Waiting:
                WaitAtPatrolPoint();
                break;
        }
        
        UpdateAnimations();
    }
    
    void Patrol()
    {
        if (patrolPoints.Length == 0)
        {
            // Simple back and forth patrol
            if (transform.position.x <= startPosition.x - 2f)
            {
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
                FlipSprite(true);
            }
            else if (transform.position.x >= startPosition.x + 2f)
            {
                rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
                FlipSprite(false);
            }
            else
            {
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
                FlipSprite(true);
            }
        }
        else
        {
            // Patrol between points
            Vector2 targetPoint = patrolPoints[currentPatrolIndex].position;
            Vector2 direction = (targetPoint - (Vector2)transform.position).normalized;
            
            if (Vector2.Distance(transform.position, targetPoint) < 0.5f)
            {
                currentState = EnemyState.Waiting;
                waitTimer = 0f;
                isWaiting = true;
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
                FlipSprite(direction.x > 0);
            }
        }
    }
    
    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed * 1.5f, rb.velocity.y);
        FlipSprite(direction.x > 0);
    }
    
    void AttackPlayer()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
        
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            // Perform attack
            if (audioSource != null && attackSound != null)
                audioSource.PlayOneShot(attackSound);
                
            lastAttackTime = Time.time;
            
            // Deal damage to player if in range
            if (Vector2.Distance(transform.position, player.position) <= attackRange)
            {
                PlayerController playerController = player.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    // Player takes damage through collision detection
                }
            }
        }
    }
    
    void WaitAtPatrolPoint()
    {
        waitTimer += Time.deltaTime;
        
        if (waitTimer >= waitTime)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            currentState = EnemyState.Patrolling;
            isWaiting = false;
        }
    }
    
    void UpdateAnimations()
    {
        if (animator != null)
        {
            animator.SetBool("IsMoving", Mathf.Abs(rb.velocity.x) > 0.1f);
            animator.SetBool("IsChasing", currentState == EnemyState.Chasing);
            animator.SetBool("IsAttacking", currentState == EnemyState.Attacking);
        }
    }
    
    void FlipSprite(bool facingRight)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = !facingRight;
        }
    }
    
    public void TakeDamage()
    {
        if (isDead) return;
        
        isDead = true;
        rb.velocity = Vector2.zero;
        
        if (audioSource != null && deathSound != null)
            audioSource.PlayOneShot(deathSound);
            
        if (animator != null)
            animator.SetTrigger("Die");
            
        // Disable collider and destroy after animation
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 1f);
    }
    
    void OnDrawGizmosSelected()
    {
        // Draw detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        // Draw attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        
        // Draw patrol points
        if (patrolPoints != null)
        {
            Gizmos.color = Color.blue;
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                if (patrolPoints[i] != null)
                {
                    Gizmos.DrawWireSphere(patrolPoints[i].position, 0.5f);
                    if (i < patrolPoints.Length - 1 && patrolPoints[i + 1] != null)
                    {
                        Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i + 1].position);
                    }
                }
            }
        }
    }
}
