using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActiveZoneTrigger : MonoBehaviour
{
    [Header("Activation Settings")]
    public float activationTime = 3f;
    public float requiredYRotation = 180f;
    public float rotationTolerance = 15f;

    [Header("References")]
    public Transform player;
    public Transform platformsParent;
    public Light[] glowLights;
    public AudioSource activationAudio;
    public AudioClip audioClipCat;
    public AudioClip audioClipPlat;

    [Header("Platform Raise Settings")]
    public float raiseHeight = 3f;
    public float raiseDuration = 1f;

    private float progress = 0f;
    private bool playerInZone = false;
    private bool activated = false;
    private Vector3[] initialPlatformPositions;

    public Image textImage;
    public float fadeDuration = 1f;
    public float delayBeforeFadeIn = 2f;

    private bool hasFadedIn = false;
    private bool hasFadedOut = false;

    void Start()
    {
        // Store original positions
        initialPlatformPositions = new Vector3[platformsParent.childCount];
        for (int i = 0; i < platformsParent.childCount; i++)
        {
            initialPlatformPositions[i] = platformsParent.GetChild(i).position;
        }

        // Initialize lights
        foreach (var light in glowLights)
        {
            light.intensity = 0f;
        }
    }

    void Update()
    {
        if (playerInZone && !activated)
        {
            float playerYRot = player.eulerAngles.y;
            float angleDifference = Mathf.Abs(Mathf.DeltaAngle(playerYRot, requiredYRotation));

            if (angleDifference <= rotationTolerance)
            {
                progress += Time.deltaTime;

                // Gradually increase light intensity
                float normalizedProgress = Mathf.Clamp01(progress / activationTime);
                foreach (var light in glowLights)
                {
                    light.intensity = Mathf.Lerp(0f, 8f, normalizedProgress); // adjust max intensity
                }

                // Play sound if not already
                if (!activationAudio.isPlaying)
                    activationAudio.PlayOneShot(audioClipCat);

                if (progress >= activationTime)
                {
                    StartCoroutine(RaisePlatforms());
                    activated = true;
                }
            }
            else
            {
                ResetActivation();
            }
        }
        else if (!playerInZone && !activated)
        {
            ResetActivation();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInZone = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
        }
            
    }

    void ResetActivation()
    {
        progress = 0f;

        foreach (var light in glowLights)
        {
            light.intensity = 0f;
        }

        if (activationAudio.isPlaying)
            activationAudio.Stop();
    }

    System.Collections.IEnumerator RaisePlatforms()
    {
        Vector3[] targetPositions = new Vector3[initialPlatformPositions.Length];
        for (int i = 0; i < initialPlatformPositions.Length; i++)
        {
            targetPositions[i] = initialPlatformPositions[i] + new Vector3(0f, raiseHeight, 0f);
        }

        activationAudio.PlayOneShot(audioClipPlat);

       // StartCoroutine(FadeInImage(delayBeforeFadeIn));

        float elapsed = 0f;
        while (elapsed < raiseDuration)
        {
            for (int i = 0; i < platformsParent.childCount; i++)
            {
                Transform platform = platformsParent.GetChild(i);
                platform.gameObject.SetActive(true);
                platform.position = Vector3.Lerp(initialPlatformPositions[i], targetPositions[i], elapsed / raiseDuration);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }
        /*
        if (hasFadedIn && !hasFadedOut)
        {
            hasFadedOut = true;
            StartCoroutine(FadeOutImage());
        }
        */
        // Final position correction
        for (int i = 0; i < platformsParent.childCount; i++)
        {
            platformsParent.GetChild(i).position = targetPositions[i];
        }

        
    }

    /*
    private IEnumerator FadeInImage(float delay)
    {
        yield return new WaitForSeconds(delay);

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            SetImageAlpha(alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }
        SetImageAlpha(1f);
        hasFadedIn = true;
    }

    private IEnumerator FadeOutImage()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            SetImageAlpha(alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }
        SetImageAlpha(0f);
    }

    private void SetImageAlpha(float alpha)
    {
        if (textImage != null)
        {
            Color c = textImage.color;
            textImage.color = new Color(c.r, c.g, c.b, alpha);
        }
    }
    */
}

