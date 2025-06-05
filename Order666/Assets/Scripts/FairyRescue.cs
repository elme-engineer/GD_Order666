using System.Collections;
using UnityEngine;

public class FairyRescue : MonoBehaviour
{
    public enum RescueReason { Fall, Fryer }

    [Header("Safe Points")]
    public Transform fryerSafePoint; // Only used for fryer
    private Transform safePoint;

    [Header("References")]
    public Transform player;
    public ParticleSystem fairyParticles;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip rescueSound;

    [Header("Settings")]
    public float fallThreshold = -15f;
    public float flySpeed = 5f;
    public float carryHeight = 2f;
    public float rescueDelay = 0.5f;

    private bool isRescuing = false;
    private PlayerController playerController;
    private bool wasPlayerControllerEnabled;
    private Light fairyLight;

    void Start()
    {
        // Cache PlayerController on player
        playerController = player.GetComponent<PlayerController>();
        if (playerController == null)
            Debug.LogError("PlayerController not found on the player!");

        // Get the Light component on this GameObject
        fairyLight = GetComponent<Light>();
        if (fairyLight == null)
            Debug.LogWarning("No Light component found on the fairy!");

        // Check if AudioSource is assigned
        if (audioSource == null)
            Debug.LogWarning("AudioSource not assigned in Inspector!");

        SetFairyEffects(false);
    }

    void Update()
    {
        if (!isRescuing && player.position.y < fallThreshold)
        {
            Debug.Log("Fall detected - initiating rescue");
            SetDynamicSafePointFromFall();
            StartCoroutine(RescuePlayer(RescueReason.Fall));
        }
    }

    public void TriggerFryerRescue()
    {
        if (!isRescuing)
        {
            Debug.Log("Fryer touched - initiating fryer rescue");
            safePoint = fryerSafePoint;
            StartCoroutine(RescuePlayer(RescueReason.Fryer));
        }
    }

    private void SetDynamicSafePointFromFall()
    {
        RaycastHit hit;
        Vector3 checkPosition = player.position + Vector3.up * 2f;

        if (Physics.Raycast(checkPosition, Vector3.down, out hit, 100f))
        {
            // Adjust offset direction based on player's current position
            float offsetX = player.position.x < 0 ? 10f : -10f;
            float offsetZ = player.position.z < 0 ? 10f : -10f;
            float offsetY = 20f;

            safePoint = new GameObject("DynamicSafePoint").transform;
            safePoint.position = new Vector3(
                player.position.x + offsetX,
                hit.point.y + offsetY,
                player.position.z + offsetZ
            );

            Debug.Log($"✅ Safe point set near fall position at {safePoint.position}");
        }
        else
        {
            Debug.LogWarning("⚠️ No ground detected - using fallback at Y = 0");
            safePoint = new GameObject("FallbackSafePoint").transform;
            safePoint.position = new Vector3(player.position.x, 0f, player.position.z);
        }
    }


    IEnumerator RescuePlayer(RescueReason reason)
    {
        isRescuing = true;
        SetFairyEffects(true);

        // Play sound
        if (audioSource != null && rescueSound != null)
        {
            audioSource.clip = rescueSound;
            audioSource.Play();
        }

        // Disable PlayerController
        wasPlayerControllerEnabled = (playerController != null && playerController.enabled);
        if (playerController != null)
            playerController.enabled = false;

        // Move fairy above player
        Vector3 fairyTargetPosition = player.position + Vector3.up * carryHeight;
        while (Vector3.Distance(transform.position, fairyTargetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                fairyTargetPosition,
                flySpeed * Time.deltaTime * 2f);
            yield return null;
        }

        yield return new WaitForSeconds(rescueDelay);

        // Move player and fairy to safe point
        float rescueStartTime = Time.time;
        Vector3 rescueStartPosition = player.position;
        float journeyLength = Vector3.Distance(rescueStartPosition, safePoint.position);

        while (Vector3.Distance(player.position, safePoint.position) > 0.1f)
        {
            float distanceCovered = (Time.time - rescueStartTime) * flySpeed;
            float fractionOfJourney = distanceCovered / journeyLength;

            player.position = Vector3.Lerp(rescueStartPosition, safePoint.position, fractionOfJourney);
            transform.position = player.position + Vector3.up * carryHeight;

            if (Time.time - rescueStartTime > 10f)
            {
                Debug.LogWarning("Rescue taking too long - forcing completion.");
                break;
            }

            yield return null;
        }

        // Final position correction
        player.position = safePoint.position;
        transform.position = safePoint.position + Vector3.up * carryHeight;

        // Re-enable player control
        if (wasPlayerControllerEnabled && playerController != null)
            playerController.enabled = true;

        // Stop sound
        if (audioSource != null && audioSource.isPlaying)
            audioSource.Stop();

        SetFairyEffects(false);
        isRescuing = false;

        // Clean up dynamic safe point
        if (reason == RescueReason.Fall && safePoint != fryerSafePoint)
        {
            Destroy(safePoint.gameObject);
        }
    }

    private void SetFairyEffects(bool active)
    {
        if (fairyParticles != null)
        {
            if (active && !fairyParticles.isPlaying)
                fairyParticles.Play();
            else if (!active && fairyParticles.isPlaying)
                fairyParticles.Stop();
        }

        if (fairyLight != null)
            fairyLight.enabled = active;
    }
}





