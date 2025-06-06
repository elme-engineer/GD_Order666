using System.Collections;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 10;
    public AudioSource activationAudio;
    public AudioClip audioClipAmmoCollect;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStatus status = other.GetComponentInChildren<PlayerStatus>();

            if (status.currentAmmo == status.maxAmmo)
                return;

            if (status != null)
            {
                GetComponent<Collider>().enabled = false;
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