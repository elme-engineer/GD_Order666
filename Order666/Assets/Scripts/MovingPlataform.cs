using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveDistance = 2f;
    public float moveSpeed = 2f;
    public float delayBetweenMoves = 2f;
    public float startDelayTime = 0f;

    [Header("Platform Link")]
    public MovingPlatform pairedPlatform;

    private Vector3 startPos;
    private Vector3 upPos;
    private Vector3 downPos;
    private bool movingUp = true;
    public bool isMaster = false;

    void Start()
    {
        startPos = transform.position;
        upPos = startPos + Vector3.up * moveDistance;
        downPos = startPos;

        if (isMaster)
            StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine()
    {
        // Optional delay before starting
        if (startDelayTime > 0f)
            yield return new WaitForSeconds(startDelayTime);

        while (true)
        {
            // Toggle direction
            movingUp = !movingUp;

            // Start movement on both platforms
            if (pairedPlatform != null)
                pairedPlatform.StartCoroutine(pairedPlatform.MoveOneDirection(!movingUp)); // Opposite

            yield return StartCoroutine(MoveOneDirection(movingUp));

            yield return new WaitForSeconds(delayBetweenMoves);
        }
    }

    public IEnumerator MoveOneDirection(bool moveUp)
    {
        Vector3 start = transform.position;
        Vector3 end = moveUp ? upPos : downPos;

        float elapsed = 0f;
        float duration = 1f / moveSpeed;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
    }
}
