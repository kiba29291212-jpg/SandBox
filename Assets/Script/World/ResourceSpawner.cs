using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] private GameObject[] resourcePrefabs;

    [Header("Spawn Settings")]
    [SerializeField] private int resourceCount = 20;
    [SerializeField] private float minDistance = 3f;

    private int chunkSize;

    public void Spawn(Vector2Int chunkCoordinate, int chunkSize, int seed)
    {
        this.chunkSize = chunkSize;

        Debug.Log("RESOURCE SPAWN START");

        Random.InitState(seed + chunkCoordinate.x * 10000 + chunkCoordinate.y);

        SpawnResources();
    }

    private void SpawnResources()
    {
        if (resourcePrefabs == null || resourcePrefabs.Length == 0)
        {
            Debug.LogError("NO RESOURCE PREFAB!");
            return;
        }

        int spawnedCount = 0;
        int attempts = 0;

        while (spawnedCount < resourceCount && attempts < resourceCount * 10)
        {
            attempts++;

            float x = Random.Range(2f, chunkSize - 2f);
            float z = Random.Range(2f, chunkSize - 2f);

            // Tạm thời đặt cây ở độ cao cố định
            float y = 10f;

            Vector3 spawnPosition = transform.position + new Vector3(x, y, z);

            if (!CanSpawn(spawnPosition))
            {
                continue;
            }

            GameObject prefab = resourcePrefabs[Random.Range(0, resourcePrefabs.Length)];

            Instantiate(
                prefab,
                spawnPosition,
                Quaternion.Euler(0f, Random.Range(0f, 360f), 0f),
                transform
            );

            spawnedCount++;

            Debug.Log("TREE SPAWNED: " + spawnedCount);
        }

        Debug.Log("TOTAL TREES: " + spawnedCount);
    }

    private bool CanSpawn(Vector3 position)
    {
        foreach (Transform child in transform)
        {
            if (Vector3.Distance(position, child.position) < minDistance)
            {
                return false;
            }
        }

        return true;
    }
}