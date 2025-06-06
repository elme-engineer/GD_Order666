using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ObamaPickup : MonoBehaviour
{
    [Header("How much Dream to restore when eaten")]
    public float dreamRestoreAmount = 50f;
    private PlayerStatus playerStatus;
    public AudioSource audioSource;
    public AudioClip audioCollect;

    private void Awake()
    {
        // Make sure this collider is a trigger
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // ← Insert a Debug.Log here to see who entered the trigger
        //Debug.Log($"ObamaPickup triggered by: {other.name} (tag = {other.tag})");

        if (!other.CompareTag("Player"))
            return;

        // If we do hit the player, log again to ensure we found PlayerStatus
        //Debug.Log("ObamaPickup detected Player. Now looking for PlayerStatus...");

        playerStatus = other.GetComponentInChildren<PlayerStatus>();
        if (playerStatus != null && playerStatus.DreamMeter != playerStatus.MaxDreamMeter) 
        {
            //Debug.Log($"Found PlayerStatus on {playerStatus.gameObject.name}. Restoring {dreamRestoreAmount} HP.");
            GetComponent<Collider>().enabled = false;
            GetComponentInChildren<Renderer>().enabled = !GetComponentInChildren<Renderer>().enabled;
            playerStatus.RestoreDream(dreamRestoreAmount);
            StartCoroutine(DestroyAfterSound());
        }
        else
        {
          //  Debug.LogWarning("ObamaPickup: PlayerStatus component was null!");
        }


    }

    IEnumerator DestroyAfterSound()
    {
        audioSource.PlayOneShot(audioCollect);
        yield return new WaitForSeconds(audioCollect.length);
        Destroy(gameObject);
    }
}
