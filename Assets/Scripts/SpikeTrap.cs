using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [Header("Trap Settings")]
    public float damage = 1;
    public float activationDelay = 0.5f;
    public float resetTime = 2f;
    
    [Header("Visual")]
    public Sprite activeSprite;
    public Sprite inactiveSprite;
    
    [Header("Audio")]
    public AudioClip activationSound;
    public AudioClip damageSound;
    
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private bool isActive = false;
    private bool isTriggered = false;
    private float triggerTimer = 0f;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        
        // Start with inactive sprite
        if (spriteRenderer != null && inactiveSprite != null)
        {
            spriteRenderer.sprite = inactiveSprite;
        }
    }
    
    void Update()
    {
        if (isTriggered && !isActive)
        {
            triggerTimer += Time.deltaTime;
            if (triggerTimer >= activationDelay)
            {
                ActivateTrap();
            }
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            TriggerTrap();
        }
    }
    
    void TriggerTrap()
    {
        isTriggered = true;
        triggerTimer = 0f;
        
        if (audioSource != null && activationSound != null)
            audioSource.PlayOneShot(activationSound);
    }
    
    void ActivateTrap()
    {
        isActive = true;
        
        // Change sprite to active
        if (spriteRenderer != null && activeSprite != null)
        {
            spriteRenderer.sprite = activeSprite;
        }
        
        // Check if player is still in range
        Collider2D playerCollider = GetComponent<Collider2D>();
        if (playerCollider != null)
        {
            Collider2D[] colliders = new Collider2D[10];
            ContactFilter2D filter = new ContactFilter2D();
            filter.SetLayerMask(LayerMask.GetMask("Player"));
            
            int count = playerCollider.OverlapCollider(filter, colliders);
            for (int i = 0; i < count; i++)
            {
                if (colliders[i].CompareTag("Player"))
                {
                    DamagePlayer(colliders[i].gameObject);
                    break;
                }
            }
        }
        
        // Reset trap after some time
        Invoke("ResetTrap", resetTime);
    }
    
    void DamagePlayer(GameObject player)
    {
        if (audioSource != null && damageSound != null)
            audioSource.PlayOneShot(damageSound);
            
        // Deal damage to player
        if (GameManager.Instance != null)
        {
            GameManager.Instance.TakeDamage();
        }
    }
    
    void ResetTrap()
    {
        isActive = false;
        isTriggered = false;
        triggerTimer = 0f;
        
        // Change sprite back to inactive
        if (spriteRenderer != null && inactiveSprite != null)
        {
            spriteRenderer.sprite = inactiveSprite;
        }
    }
    
    void OnDrawGizmosSelected()
    {
        // Draw activation area
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, GetComponent<Collider2D>().bounds.size);
    }
}
