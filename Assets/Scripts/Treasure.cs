using UnityEngine;

public class Treasure : MonoBehaviour
{
    [Header("Treasure Settings")]
    public int value = 1;
    public float bobSpeed = 2f;
    public float bobHeight = 0.5f;
    public bool isCollected = false;
    
    [Header("Visual Effects")]
    public GameObject collectEffect;
    public AudioClip collectSound;
    
    private Vector3 startPosition;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private Collider2D treasureCollider;
    
    void Start()
    {
        startPosition = transform.position;
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        treasureCollider = GetComponent<Collider2D>();
    }
    
    void Update()
    {
        if (!isCollected)
        {
            // Bobbing animation
            float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            CollectTreasure();
        }
    }
    
    void CollectTreasure()
    {
        isCollected = true;
        
        // Play collect sound
        if (audioSource != null && collectSound != null)
            audioSource.PlayOneShot(collectSound);
        
        // Spawn collect effect
        if (collectEffect != null)
        {
            GameObject effect = Instantiate(collectEffect, transform.position, Quaternion.identity);
            Destroy(effect, 2f);
        }
        
        // Notify game manager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CollectTreasure();
        }
        
        // Disable visual and collider
        if (spriteRenderer != null)
            spriteRenderer.enabled = false;
        if (treasureCollider != null)
            treasureCollider.enabled = false;
        
        // Destroy after a short delay to allow sound to play
        Destroy(gameObject, 0.5f);
    }
    
    void OnDrawGizmosSelected()
    {
        // Draw bobbing range
        Gizmos.color = Color.yellow;
        Vector3 topPos = transform.position + Vector3.up * bobHeight;
        Vector3 bottomPos = transform.position - Vector3.up * bobHeight;
        Gizmos.DrawLine(topPos, bottomPos);
    }
}
