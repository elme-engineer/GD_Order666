using UnityEngine;

public class OilDamageZone : MonoBehaviour
{
    public float dreamDamage = 10f;
    public float damageInterval = 0.5f;
    private float lastDamageTime;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null && Time.time - lastDamageTime >= damageInterval)
            {
                // Deal dream damage
                var status = player.GetComponentInChildren<PlayerStatus>();
                if (status != null)
                {
                    status.TakeDreamDamage(dreamDamage);
                    lastDamageTime = Time.time;
                }

                // Play sound
                var soundScript = GetComponent<PlaySoundOnCollision>();
                if (soundScript != null)
                {
                    soundScript.PlaySound();
                }
            }
        }
    }
}

