using UnityEngine;

public class FairySparklesFollower : MonoBehaviour
{
    [Header("References")]
    public Transform fairy; // Assign the Fairy's transform
    public Vector3 positionOffset = Vector3.zero; // Adjust if needed

    [Header("Smooth Follow Settings")]
    public float followSpeed = 10f;
    public bool smoothFollow = true;

    private ParticleSystem sparkles;
    private Vector3 targetPosition;

    void Start()
    {
        sparkles = GetComponent<ParticleSystem>();
        if (fairy == null)
        {
            Debug.LogError("Fairy transform not assigned!");
            enabled = false;
            return;
        }

        // Initialize position
        transform.position = fairy.position + positionOffset;
    }

    void Update()
    {
        if (fairy == null) return;

        // Calculate target position (center of fairy + offset)
        targetPosition = fairy.position + positionOffset;

        // Move particles to follow fairy
        if (smoothFollow)
        {
            // Smooth movement
            transform.position = Vector3.Lerp(
                transform.position,
                targetPosition,
                followSpeed * Time.deltaTime
            );
        }
        else
        {
            // Instant follow
            transform.position = targetPosition;
        }

        // Optional: Rotate to always face camera (if needed)
        // transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
    }
}
