using UnityEngine;

public class CollectibleEffect : MonoBehaviour
{
    [Header("Effect Settings")]
    public float lifetime = 2f;
    public float rotationSpeed = 90f;
    public float scaleSpeed = 2f;
    public float maxScale = 1.5f;
    
    [Header("Particle Effect")]
    public GameObject particleEffect;
    
    private Vector3 originalScale;
    private float timer = 0f;
    
    void Start()
    {
        originalScale = transform.localScale;
        
        // Spawn particle effect if available
        if (particleEffect != null)
        {
            GameObject effect = Instantiate(particleEffect, transform.position, Quaternion.identity);
            Destroy(effect, lifetime);
        }
    }
    
    void Update()
    {
        timer += Time.deltaTime;
        
        // Rotate the effect
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        
        // Scale up and down
        float scaleMultiplier = Mathf.Sin(timer * scaleSpeed) * 0.5f + 1f;
        transform.localScale = originalScale * scaleMultiplier;
        
        // Fade out over time
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = Mathf.Lerp(1f, 0f, timer / lifetime);
            spriteRenderer.color = color;
        }
        
        // Destroy after lifetime
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
