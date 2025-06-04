using System.Collections;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 5;
    public AudioSource activationAudio;
    public AudioClip audioClipAmmoCollect;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStatus status = other.GetComponentInChildren<PlayerStatus>();
            if (status != null)
            {
                GetComponent<Renderer>().enabled = !GetComponent<Renderer>().enabled;
                status.AddAmmo(ammoAmount);
                StartCoroutine(DestroyAfterSound());
               // Debug.Log("Ammo collected. New ammo: " + status.currentAmmo);
            }
        }
    }


    IEnumerator DestroyAfterSound()
    {
        activationAudio.PlayOneShot(audioClipAmmoCollect);
        yield return new WaitForSeconds(audioClipAmmoCollect.length);
        Destroy(gameObject);
    }
}