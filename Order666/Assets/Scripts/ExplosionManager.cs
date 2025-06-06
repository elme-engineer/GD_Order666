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
        // 1) Instantiate and play the explosion effect
        ParticleSystem explosionInstance = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosionInstance.Play();

        // 2) Wait for the explosion’s particle duration
        yield return new WaitForSeconds(explosionInstance.main.duration);

        // 3) Spawn an Obama prefab slightly above the explosion point
        if (obamaPrefab != null)
        {
            Vector3 spawnPos = new Vector3(
                position.x,
                position.y + obamaSpawnHeight,
                position.z
            );
            Instantiate(obamaPrefab, spawnPos, Quaternion.identity);
        }

        // 4) Clean up the particle object after a delay
        Destroy(explosionInstance.gameObject, cleanupDelay);

        // ———— REMOVE THIS BLOCK ————
        // We used to call StartNextWave() here, but that no longer exists in RandomBabySpawner.
        // If you want “explosions” to trigger new waves, add your own public method there.
        //
        // if (spawner != null && spawner.isActiveAndEnabled)
        // {
        //     spawner.StartNextWave();
        // }
        // ————————————————————————
    }
}
