using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformSwapController : MonoBehaviour
{
    [Header("Platform Arrays")]
    public Transform[] nickiPlatforms;
    public Transform[] friesPlatforms;

    [Header("Settings")]
    public float moveAmount = 1.3f;
    public float moveDuration = 1f;
    public float waitTime = 2f;

    private Vector3[] nickiStartPositions;
    private Vector3[] friesStartPositions;

    private void Start()
    {
        if (nickiPlatforms.Length != friesPlatforms.Length)
        {
            Debug.LogError("Top and Bottom platform arrays must be the same length.");
            enabled = false;
            return;
        }

        // Store starting positions
        nickiStartPositions = new Vector3[nickiPlatforms.Length];
        friesStartPositions = new Vector3[friesPlatforms.Length];

        for (int i = 0; i < nickiPlatforms.Length; i++)
        {
            nickiStartPositions[i] = nickiPlatforms[i].position;
            friesStartPositions[i] = friesPlatforms[i].position;
        }

        StartCoroutine(SwapCycle());
    }

    IEnumerator SwapCycle()
    {
        while (true)
        {
            // Move down tops, up bottoms
            yield return StartCoroutine(MovePlatforms(true));
            yield return new WaitForSeconds(waitTime);

            // Return to original
            yield return StartCoroutine(MovePlatforms(false));
            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator MovePlatforms(bool swap)
    {
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            float t = elapsed / moveDuration;

            for (int i = 0; i < nickiPlatforms.Length; i++)
            {
                nickiPlatforms[i].position = Vector3.Lerp(
                    swap ? nickiStartPositions[i] : nickiStartPositions[i] + Vector3.down * moveAmount,
                    swap ? nickiStartPositions[i] + Vector3.down * moveAmount : nickiStartPositions[i],
                    t
                );

                friesPlatforms[i].position = Vector3.Lerp(
                    swap ? friesStartPositions[i] : friesStartPositions[i] + Vector3.up * moveAmount,
                    swap ? friesStartPositions[i] + Vector3.up * moveAmount : friesStartPositions[i],
                    t
                );
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Final correction
        for (int i = 0; i < nickiPlatforms.Length; i++)
        {
            nickiPlatforms[i].position = swap
                ? nickiStartPositions[i] + Vector3.down * moveAmount
                : nickiStartPositions[i];

            friesPlatforms[i].position = swap
                ? friesStartPositions[i] + Vector3.up * moveAmount
                : friesStartPositions[i];
        }
    }
}
