using System.Collections;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    [Header("References")]
    public RandomBabySpawner spawner;

    [Header("Settings")]
    public float obamaSpawnHeight = 1f;
    public float cleanupDelay = 1f;

    public void HandleExplosion(Vector3 position, ParticleSystem explosionPrefab, GameObject obamaPrefab)
    {
        if (this == null || !this.isActiveAndEnabled) return;

        StartCoroutine(ExplosionSequence(position, explosionPrefab, obamaPrefab));
    }

    private IEnumerator ExplosionSequence(Vector3 position, ParticleSystem explosionPrefab, GameObject obamaPrefab)
    {
        // Instantiate new explosion from prefab
        ParticleSystem explosionInstance = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosionInstance.Play();

        // Wait for explosion to finish
        yield return new WaitForSeconds(explosionInstance.main.duration);

        // Spawn Obama from prefab
        if (obamaPrefab != null)
        {
            Vector3 spawnPos = new Vector3(
                position.x,
                position.y + obamaSpawnHeight,
                position.z
            );
            Instantiate(obamaPrefab, spawnPos, Quaternion.identity);
        }

        // Clean up
        Destroy(explosionInstance.gameObject, cleanupDelay);

        // Trigger next wave if spawner exists
        if (spawner != null && spawner.isActiveAndEnabled)
        {
            spawner.StartNextWave();
        }
    }
}