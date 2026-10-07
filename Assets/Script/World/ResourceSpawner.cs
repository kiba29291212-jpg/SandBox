using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField] private GameObject[] resourcePrefabs;

    [Header("Spawn Settings")]
    [SerializeField] private int resourceCount = 20;
    [SerializeField] private float minDistance = 3f;

    [Header("Ground")]
    [SerializeField] private float raycastHeight = 100f;

    private int chunkSize;
    private MeshCollider terrainCollider;

    public void Spawn(Vector2Int chunkCoordinate, int chunkSize, int seed)
    {
        this.chunkSize = chunkSize;
        terrainCollider = GetComponent<MeshCollider>();

        if (terrainCollider == null)
        {
            Debug.LogError("NO TERRAIN MESH COLLIDER!");
            return;
        }

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

            Vector3 rayOrigin = transform.position + new Vector3(x, raycastHeight, z);

            if (!Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, raycastHeight * 2f))
            {
                continue;
            }

            if (hit.collider != terrainCollider)
            {
                continue;
            }

            Vector3 spawnPosition = hit.point;

            if (!CanSpawn(spawnPosition))
            {
                continue;
            }

            GameObject prefab = resourcePrefabs[Random.Range(0, resourcePrefabs.Length)];

            GameObject resource = Instantiate(
                prefab,
                spawnPosition,
                Quaternion.Euler(0f, Random.Range(0f, 360f), 0f),
                transform
            );

            AlignToGround(resource, hit.point);

            spawnedCount++;
        }

        Debug.Log("TOTAL RESOURCES: " + spawnedCount);
    }

    private void AlignToGround(GameObject resource, Vector3 groundPosition)
    {
        Renderer[] renderers = resource.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
        {
            return;
        }

        Bounds bounds = renderers[0].bounds;

        foreach (Renderer renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }

        float offset = groundPosition.y - bounds.min.y;

        resource.transform.position += Vector3.up * offset;
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