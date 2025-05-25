using System.Collections;
using UnityEngine;

public class FairyRescue : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform safePoint;
    public ParticleSystem fairyParticles;

    [Header("Audio")]
    public AudioSource audioSource; // ← Assign this in Inspector
    public AudioClip rescueSound;   // ← Assign your sound clip here

    [Header("Settings")]
    public float fallThreshold = -20f;
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
            StartCoroutine(RescuePlayer());
        }
    }

    IEnumerator RescuePlayer()
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


