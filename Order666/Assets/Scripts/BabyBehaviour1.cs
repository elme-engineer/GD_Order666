using UnityEngine;

public class BabyBehaviour : MonoBehaviour
{
    [Header("Effects")]
    public ParticleSystem explosionEffectPrefab;
    public AudioClip explosionSound;

    private GameObject obamaPrefab;
    private ExplosionManager explosionManager;
    private bool hasExploded = false;

    public void Setup(GameObject obamaPrefab)
    {
        this.obamaPrefab = obamaPrefab;
        explosionManager = FindObjectOfType<ExplosionManager>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasExploded) return;
        hasExploded = true;
        Explode(collision);
    }

    private void Explode(Collision collision)
    {
        // Disable physics
        if (TryGetComponent<Collider>(out var collider))
            collider.enabled = false;

        if (TryGetComponent<Rigidbody>(out var rb))
            rb.isKinematic = true;

        // Get collision point
        ContactPoint contact = collision.contacts[0];

        // Play sound
        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, contact.point);
        }

        // Hide baby
        GetComponent<Renderer>().enabled = false;

        // Notify explosion manager
        if (explosionManager != null && explosionEffectPrefab != null)
        {
            explosionManager.HandleExplosion(
                contact.point,
                explosionEffectPrefab,
                obamaPrefab
            );
        }

        // Destroy baby after short delay
        Destroy(gameObject, 1f);
    }
}