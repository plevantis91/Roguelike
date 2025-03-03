using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;
    public float followSpeed = 2f;
    
    [Header("Camera Bounds")]
    public bool useBounds = true;
    public float leftBound = -10f;
    public float rightBound = 10f;
    public float topBound = 5f;
    public float bottomBound = -5f;
    
    [Header("Smoothing")]
    public bool smoothFollow = true;
    public float smoothTime = 0.3f;
    
    private Vector3 velocity = Vector3.zero;
    private Vector3 offset;
    private Camera cam;
    
    void Start()
    {
        cam = GetComponent<Camera>();
        
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
        }
        
        // Calculate initial offset
        if (target != null)
        {
            offset = transform.position - target.position;
        }
    }
    
    void LateUpdate()
    {
        if (target == null) return;
        
        Vector3 targetPosition = target.position + offset;
        
        // Apply bounds if enabled
        if (useBounds)
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, leftBound, rightBound);
            targetPosition.y = Mathf.Clamp(targetPosition.y, bottomBound, topBound);
        }
        
        // Move camera
        if (smoothFollow)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }
    
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        if (target != null)
        {
            offset = transform.position - target.position;
        }
    }
    
    public void SetBounds(float left, float right, float top, float bottom)
    {
        leftBound = left;
        rightBound = right;
        topBound = top;
        bottomBound = bottom;
    }
    
    void OnDrawGizmosSelected()
    {
        if (useBounds)
        {
            Gizmos.color = Color.red;
            Vector3 center = new Vector3((leftBound + rightBound) / 2, (topBound + bottomBound) / 2, 0);
            Vector3 size = new Vector3(rightBound - leftBound, topBound - bottomBound, 0);
            Gizmos.DrawWireCube(center, size);
        }
    }
}
