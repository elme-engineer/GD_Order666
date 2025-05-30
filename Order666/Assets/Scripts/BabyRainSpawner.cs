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
    public int maxTotalBabies = 30; // 🆕 Max total number of babies
    private int totalBabiesSpawned = 0; // 🆕 Count of babies spawned

    private bool isSpawning = false;

    void Start()
    {
        StartNextWave();
    }

    public void StartNextWave()
    {
        if (!isSpawning && totalBabiesSpawned < maxTotalBabies)
        {
            StartCoroutine(SpawnWave());
        }
    }

    private IEnumerator SpawnWave()
    {
        isSpawning = true;

        for (int i = 0; i < babiesPerWave; i++)
        {
            if (totalBabiesSpawned >= maxTotalBabies)
                break;

            if (babyPrefab != null && obamaPrefab != null)
            {
                SpawnBaby();
            }
            yield return new WaitForSeconds(delayBetweenBabies);
        }

        isSpawning = false;

        if (continuousSpawning && totalBabiesSpawned < maxTotalBabies)
        {
            yield return new WaitForSeconds(delayBetweenWaves);
            StartNextWave();
        }
    }

    private void SpawnBaby()
    {
        Vector3 spawnPosition = transform.position + new Vector3(
            Random.Range(-spawnAreaWidth / 2f, spawnAreaWidth / 2f),
            spawnHeight,
            Random.Range(-spawnAreaWidth / 2f, spawnAreaWidth / 2f)
        );

        GameObject baby = Instantiate(babyPrefab, spawnPosition, Quaternion.identity);
        totalBabiesSpawned++; // 🆕 Increment the count

        BabyBehaviour babyScript = baby.GetComponent<BabyBehaviour>();
        if (babyScript != null)
        {
            babyScript.Setup(obamaPrefab);
        }
    }
}