using System.Collections;
using UnityEngine;

public class RandomBabySpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject babyPrefab;
    public GameObject obamaPrefab;

    [Header("Spawning Settings")]
    public int babiesPerWave = 5;
    public float delayBetweenBabies = 1f;
    public float delayBetweenWaves = 3f;
    public float spawnAreaWidth = 5f;
    public float spawnHeight = 10f;
    public bool continuousSpawning = true;

    [Header("Limits")]
    public int maxTotalBabies = 30;       // Max total babies that can ever spawn
    private int totalBabiesSpawned = 0;   // How many we’ve spawned so far

    [Header("Player Reference")]
    public PlayerStatus playerStatus;     // Drag your Player (with PlayerStatus) here

    private bool isSpawning = false;

    private void Start()
    {
        // Start a coroutine that only spawns once the player’s DreamMeter is ≤ 50%
        StartCoroutine(SpawnWhenLowLife());
    }

    /// <summary>
    /// Wait until player’s life ≤ half, then spawn a wave, wait delayBetweenWaves, and repeat
    /// until we hit maxTotalBabies.
    /// </summary>
    private IEnumerator SpawnWhenLowLife()
    {
        // Keep doing waves until we reach the total‐babies limit
        while (totalBabiesSpawned < maxTotalBabies)
        {
            // Wait until playerStatus is assigned & player’s life ≤ 50%
            if (playerStatus != null &&
                playerStatus.DreamMeter <= (playerStatus.MaxDreamMeter / 2f))
            {
                // Player is “low on life” → do one wave
                yield return StartCoroutine(SpawnWave());

                // After the wave, wait delayBetweenWaves (unless we’re already at max)
                if (totalBabiesSpawned < maxTotalBabies && continuousSpawning)
                {
                    yield return new WaitForSeconds(delayBetweenWaves);
                }
            }
            else
            {
                // Not low enough yet (or playerStatus is missing) → recheck in 1 second
                yield return new WaitForSeconds(1f);
            }
        }
        // Once we hit maxTotalBabies, the loop ends, and no more spawning happens
    }

    /// <summary>
    /// Spawns “babiesPerWave” baby Prefabs, each separated by delayBetweenBabies.
    /// </summary>
    private IEnumerator SpawnWave()
    {
        if (isSpawning)
            yield break; // Already spawning (shouldn't happen), so do nothing.

        isSpawning = true;

        for (int i = 0; i < babiesPerWave; i++)
        {
            if (totalBabiesSpawned >= maxTotalBabies)
                break;

            if (babyPrefab != null && obamaPrefab != null)
            {
                SpawnBaby();
            }

            // Wait between each baby spawn
            yield return new WaitForSeconds(delayBetweenBabies);
        }

        isSpawning = false;
    }

    /// <summary>
    /// Instantiates a single baby at a random XZ position (within spawnAreaWidth),
    /// at fixed height “spawnHeight.” Then calls Setup() so BabyBehavior knows which Obama prefab to spawn.
    /// </summary>
    private void SpawnBaby()
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-spawnAreaWidth / 2f, spawnAreaWidth / 2f),
            0f,
            Random.Range(-spawnAreaWidth / 2f, spawnAreaWidth / 2f)
        );

        Vector3 spawnPos = transform.position + randomOffset + Vector3.up * spawnHeight;
        GameObject babyGO = Instantiate(babyPrefab, spawnPos, Quaternion.identity);
        totalBabiesSpawned++;

        BabyBehaviour babyScript = babyGO.GetComponent<BabyBehaviour>();
        if (babyScript != null)
        {
            // Pass along the Obama prefab so that when the baby “explodes,” we know what to spawn
            babyScript.Setup(obamaPrefab);
        }
    }
}
