using System.Collections;
using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    public int maxHPAmount = 20;
    public AudioSource activationAudio;
    public AudioClip audioHeartCollect;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStatus status = other.GetComponentInChildren<PlayerStatus>();

            if (status != null)
            {
                GetComponent<Collider>().enabled = false;
                GetComponentInChildren<Renderer>().enabled = !GetComponentInChildren<Renderer>().enabled;
                status.AddMaxDream(maxHPAmount);
                StartCoroutine(DestroyAfterSound());
                
            }
        }
    }


    IEnumerator DestroyAfterSound()
    {
        activationAudio.PlayOneShot(audioHeartCollect);
        yield return new WaitForSeconds(audioHeartCollect.length);
        Destroy(gameObject);
    }
}
