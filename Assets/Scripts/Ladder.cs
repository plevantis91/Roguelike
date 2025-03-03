using UnityEngine;

public class Ladder : MonoBehaviour
{
    [Header("Ladder Settings")]
    public float climbSpeed = 3f;
    public bool isTopLadder = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                // Enable climbing for player
                player.GetComponent<Rigidbody2D>().gravityScale = 0f;
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                // Disable climbing for player
                player.GetComponent<Rigidbody2D>().gravityScale = 1f;
            }
        }
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Keep gravity disabled while on ladder
            other.GetComponent<Rigidbody2D>().gravityScale = 0f;
        }
    }
}
