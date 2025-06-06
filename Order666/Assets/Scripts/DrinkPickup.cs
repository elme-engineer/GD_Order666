using System.Collections;
using UnityEngine;

public class DrinkPickup : MonoBehaviour
{
    public int dmgAmount = 30;
    public AudioSource activationAudio;
    public AudioClip audioDrinkCollect;
    private bool isConsumed = false;


    void OnTriggerEnter(Collider other)
    {
        if (isConsumed || other.CompareTag("Player"))
        {
            PlayerStatus status = other.GetComponentInChildren<PlayerStatus>();

            if (status != null)
            {
                isConsumed = true;
                GetComponent<Collider>().enabled = false;
                GetComponent<Renderer>().enabled = !GetComponent<Renderer>().enabled;
                status.TakeDreamDamage(dmgAmount);
                StartCoroutine(DestroyAfterSound());

            }
        }
    }


    IEnumerator DestroyAfterSound()
    {
        activationAudio.PlayOneShot(audioDrinkCollect);
        yield return new WaitForSeconds(audioDrinkCollect.length);
        Destroy(gameObject);
    }
}
