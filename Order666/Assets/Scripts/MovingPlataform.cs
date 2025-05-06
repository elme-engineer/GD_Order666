using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour
{
    public PlatformType platformType;
    public float moveDistance = 2f;
    public float moveSpeed = 2f;
    public float delayBetweenMoves = 2f;
    public float dreamDamage = 10f;
    public float startDelayTime = 2.5f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool movingOut = false;
    private bool isSafeZone;

    private void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition + transform.forward * moveDistance;

        // Optional: static safe zones for Plane A
        isSafeZone = (platformType == PlatformType.Safe && transform.CompareTag("SafeZone"));

        if (CompareTag("FriePlat"))
        {
            StartCoroutine(DelayedStart());
        }
        else if (CompareTag("NickiPlat") && !isSafeZone)
        {
            StartCoroutine(MoveRoutine());
        }
            
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(startDelayTime);
        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            Vector3 start = movingOut ? startPosition : targetPosition;
            Vector3 end = movingOut ? targetPosition : startPosition;

            float elapsed = 0f;
            while (elapsed < 1f)
            {
                transform.position = Vector3.Lerp(start, end, elapsed);
                elapsed += Time.deltaTime * moveSpeed;
                yield return null;
            }

            transform.position = end;
            movingOut = !movingOut;

            yield return new WaitForSeconds(delayBetweenMoves);
        }
    }

    /*
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("OnControllerColliderHit: Something collided");

        if (platformType == PlatformType.Dangerous && hit.collider.CompareTag("Player"))
        {
            Debug.Log("OnControllerColliderHit: Player touched FriePlat!");

            var status = hit.controller.GetComponentInChildren<PlayerStatus>();
            if (status != null)
            {
                status.TakeDreamDamage(dreamDamage);
            }
        }
    }
    */
}
