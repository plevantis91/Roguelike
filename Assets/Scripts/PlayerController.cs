using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float climbSpeed = 3f;
    
    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    
    [Header("Audio")]
    public AudioClip jumpSound;
    public AudioClip collectSound;
    
    private Rigidbody2D rb;
    private Animator animator;
    private AudioSource audioSource;
    private bool isGrounded;
    private bool isClimbing;
    private float horizontalInput;
    private bool facingRight = true;
    
    // Animation parameters
    private const string IS_RUNNING = "IsRunning";
    private const string IS_JUMPING = "IsJumping";
    private const string IS_CLIMBING = "IsClimbing";
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        
        if (groundCheck == null)
        {
            GameObject groundCheckObj = new GameObject("GroundCheck");
            groundCheckObj.transform.SetParent(transform);
            groundCheckObj.transform.localPosition = new Vector3(0, -0.5f, 0);
            groundCheck = groundCheckObj.transform;
        }
    }
    
    void Update()
    {
        HandleInput();
        CheckGrounded();
        UpdateAnimations();
    }
    
    void FixedUpdate()
    {
        HandleMovement();
    }
    
    void HandleInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        
        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
        
        // Climbing (when touching ladder)
        isClimbing = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S);
    }
    
    void HandleMovement()
    {
        if (isClimbing)
        {
            // Climbing movement
            float verticalInput = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(0, verticalInput * climbSpeed);
        }
        else
        {
            // Normal horizontal movement
            rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        }
        
        // Flip character based on movement direction
        if (horizontalInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && facingRight)
        {
            Flip();
        }
    }
    
    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        if (jumpSound != null)
            audioSource.PlayOneShot(jumpSound);
    }
    
    void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
    
    void UpdateAnimations()
    {
        if (animator != null)
        {
            animator.SetBool(IS_RUNNING, Mathf.Abs(horizontalInput) > 0.1f && isGrounded);
            animator.SetBool(IS_JUMPING, !isGrounded);
            animator.SetBool(IS_CLIMBING, isClimbing);
        }
    }
    
    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Treasure"))
        {
            CollectTreasure(other.gameObject);
        }
        else if (other.CompareTag("Enemy"))
        {
            TakeDamage();
        }
        else if (other.CompareTag("Exit"))
        {
            GameManager.Instance.LevelComplete();
        }
    }
    
    void CollectTreasure(GameObject treasure)
    {
        if (collectSound != null)
            audioSource.PlayOneShot(collectSound);
            
        GameManager.Instance.CollectTreasure();
        Destroy(treasure);
    }
    
    void TakeDamage()
    {
        GameManager.Instance.TakeDamage();
        // Add damage effect here (flash, knockback, etc.)
    }
    
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
