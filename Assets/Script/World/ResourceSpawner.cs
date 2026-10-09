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

    private MeshCollider terrainCollider;

    public void Spawn(Vector2Int chunkCoordinate, int chunkSize, int seed, WorldGenerator worldGenerator)
    {
        terrainCollider = GetComponent<MeshCollider>();

        if (terrainCollider == null)
        {
            Debug.LogError("NO TERRAIN MESH COLLIDER!");
            return;
        }

        Random.InitState(seed + chunkCoordinate.x * 10000 + chunkCoordinate.y);

        SpawnResources(chunkSize, worldGenerator);
    }

    private void SpawnResources(int chunkSize, WorldGenerator worldGenerator)
    {
        if (resourcePrefabs == null || resourcePrefabs.Length == 0)
            return;

        int spawnedCount = 0;

        for (int i = 0; i < resourceCount; i++)
        {
            float x = Random.Range(2f, chunkSize - 2f);
            float z = Random.Range(2f, chunkSize - 2f);

            float worldX = transform.position.x + x;
            float worldZ = transform.position.z + z;

            float density = worldGenerator.GetResourceDensity(worldX, worldZ);

            if (density <= 0f || Random.value > density)
                continue;

            Vector3 rayOrigin = new Vector3(worldX, transform.position.y + raycastHeight, worldZ);

            if (!Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, raycastHeight * 2f))
                continue;

            if (hit.collider != terrainCollider)
                continue;

            if (hit.point.y <= worldGenerator.GetSeaLevel())
                continue;

            if (!CanSpawn(hit.point))
                continue;

            GameObject prefab = resourcePrefabs[Random.Range(0, resourcePrefabs.Length)];

            GameObject resource = Instantiate(prefab, hit.point, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f), transform);

            AlignToGround(resource, hit.point);

            spawnedCount++;
        }

        Debug.Log("TOTAL RESOURCES: " + spawnedCount);
    }

    private void AlignToGround(GameObject resource, Vector3 groundPosition)
    {
        Renderer[] renderers = resource.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
            return;

        Bounds bounds = renderers[0].bounds;

        foreach (Renderer renderer in renderers)
            bounds.Encapsulate(renderer.bounds);

        float offset = groundPosition.y - bounds.min.y;

        resource.transform.position += Vector3.up * offset;
    }

    private bool CanSpawn(Vector3 position)
    {
        foreach (Transform child in transform)
        {
            if (Vector3.Distance(position, child.position) < minDistance)
                return false;
        }

        return true;
    }
}