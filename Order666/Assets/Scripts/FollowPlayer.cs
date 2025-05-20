using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    private PlayerController playerController;

    public Vector3 offset = new Vector3(0, 1, -2);
    public float followSpeed = 5f;
    public float rotationSpeed = 5f;

    public Image fairyImage;
    public float fadeDuration = 1f;
    public float delayBeforeFadeIn = 2f;

    private bool hasStartedMoving = false;
    private bool hasFadedIn = false;
    private bool hasFadedOut = false;

    void Start()
    {
        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindWithTag("Player");
            if (foundPlayer != null)
            {
                player = foundPlayer.transform;
            }
            else
            {
                Debug.LogWarning("Player not found! Please assign the player Transform.");
                return;
            }
        }

        playerController = player.GetComponent<PlayerController>();
        if (playerController == null)
            playerController = player.GetComponentInChildren<PlayerController>();

        if (playerController == null)
        {
            Debug.LogError("PlayerController script not found on player object!");
            return;
        }

        // Set initial fairy position and rotation
        transform.position = player.position + (player.rotation * offset);
        transform.rotation = Quaternion.Euler(0, -162, 0);

        // Start hidden
        if (fairyImage != null)
        {
            SetImageAlpha(0f);
            StartCoroutine(FadeInFairyImage(delayBeforeFadeIn));
        }
    }

    void Update()
    {
        if (player == null || playerController == null) return;

        // Detect movement and trigger fade out
        if (hasFadedIn && !hasFadedOut && playerController.IsMoving())
        {
            hasStartedMoving = true;
            hasFadedOut = true;
            StartCoroutine(FadeOutFairyImage());
        }

        // Follow logic
        Vector3 rotatedOffset = player.rotation * offset;
        Vector3 targetPosition = player.position + rotatedOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // Rotation
        if (hasStartedMoving)
        {
            Quaternion targetRotation = player.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, -162, 0);
        }
    }

    private IEnumerator FadeInFairyImage(float delay)
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

    private IEnumerator FadeOutFairyImage()
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
        if (fairyImage != null)
        {
            Color c = fairyImage.color;
            fairyImage.color = new Color(c.r, c.g, c.b, alpha);
        }
    }
}


