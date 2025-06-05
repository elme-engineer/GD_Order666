using System.Collections;
using UnityEngine;

public class RandomBabySpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject babyPrefab;
    public GameObject obamaPrefab;

    [Header("Spawning Settings")]
    public int babiesPerWave = 3;
    public float delayBetweenBabies = 1f;
    public float delayBetweenWaves = 3f;
    public float spawnAreaWidth = 5f;
    public float spawnHeight = 10f;
    public bool continuousSpawning = true;

    [Header("Limits")]
    public int maxTotalBabies = 30;
    private int totalBabiesSpawned = 0;

    [Header("Player Reference")]
    public PlayerStatus playerStatus; 

    private bool isSpawning = false;

    void Start()
    {
        StartCoroutine(SpawnWhenLowLife());
    }

    private IEnumerator SpawnWhenLowLife()
    {
        while (totalBabiesSpawned < maxTotalBabies)
        {
            if (playerStatus != null && 
                playerStatus.DreamMeter <= (playerStatus.MaxDreamMeter / 2f))
            {
                StartNextWave();
                yield return new WaitForSeconds(delayBetweenWaves);
            }
            else
            {
                yield return new WaitForSeconds(10f); 
            }
        }
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
    }

    private void SpawnBaby()
    {
        Vector3 spawnPosition = transform.position + new Vector3(
            Random.Range(-spawnAreaWidth / 2f, spawnAreaWidth / 2f),
            spawnHeight,
            Random.Range(-spawnAreaWidth / 2f, spawnAreaWidth / 2f)
        );

        GameObject baby = Instantiate(babyPrefab, spawnPosition, Quaternion.identity);
        totalBabiesSpawned++;

        BabyBehaviour babyScript = baby.GetComponent<BabyBehaviour>();
        if (babyScript != null)
        {
            babyScript.Setup(obamaPrefab);
        }
    }
}
