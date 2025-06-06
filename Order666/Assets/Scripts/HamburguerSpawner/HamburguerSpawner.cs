using UnityEngine;

public class HamburguerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject hamburgerPrefab;
    [SerializeField] private float spawnInterval = 2.5f;

    private float timer;

    void Start()
    {

        // Spawn 3 hamburgers at the beginning
        for (int i = 0; i < 3; i++)
        {
            SpawnHamburger();
        }

        timer = spawnInterval; // Start countdown for the next one
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnHamburger();
            timer = spawnInterval;
        }
    }

    void SpawnHamburger()
    {
        Instantiate(hamburgerPrefab, transform.position, transform.rotation);
    }
}
