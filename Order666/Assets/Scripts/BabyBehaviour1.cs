using UnityEngine;

public class BabyBehaviour : MonoBehaviour
{
    [Tooltip("The explosion particle effect to play on impact")]
    public ParticleSystem explosionEffect;

    [Tooltip("The cat ammo to appear after explosion")]
    public GameObject catAmmo;

    [Tooltip("The sound to play on explosion")]
    public AudioClip explosionSound;

    private bool hasExploded = false;

    void OnCollisionEnter(Collision collision)
    {
        if (hasExploded)
            return;

        hasExploded = true;

        // Disable collider to prevent further collisions
        if (TryGetComponent<Collider>(out var collider))
            collider.enabled = false;

        // Get exact collision point & normal
        ContactPoint contact = collision.contacts[0];

        // Play explosion sound at the point of contact
        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, contact.point);
        }

        // Spawn explosion at impact point, oriented to surface normal
        ParticleSystem explosion = Instantiate(
            explosionEffect,
            contact.point,
            Quaternion.LookRotation(contact.normal)
        );
        explosion.Play();

        // Use ExplosionManager to trigger the explosion logic
        ExplosionManager explosionManager = Object.FindFirstObjectByType<ExplosionManager>();
        if (explosionManager != null)
        {
            explosionManager.TriggerExplosion(contact.point, explosion, catAmmo);
        }

        // Hide baby (disabling is safer than destroying for particle parenting)
        gameObject.SetActive(false);
    }
}
