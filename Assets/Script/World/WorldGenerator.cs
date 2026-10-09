using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player;

    [Header("Camera")]
    [SerializeField] private Camera playerCamera;

    [Header("Chunk")]
    [SerializeField] private WorldChunk chunkPrefab;
    [SerializeField] private int chunkSize = 32;

    [Header("Terrain")]
    [SerializeField] private float heightMultiplier = 20f;
    [SerializeField] private float noiseScale = 0.02f;
    [SerializeField] private float terrainDetailHeight = 4f;
    [SerializeField] private int seed = 12345;

    [Header("Mountains")]
    [SerializeField] private float mountainRegionScale = 0.0015f;
    [SerializeField] private float mountainScale = 0.012f;
    [SerializeField] private float mountainHeight = 30f;

    [Header("Biomes")]
    [SerializeField] private float continentScale = 0.004f;
    [SerializeField] private float biomeScale = 0.0006f;
    [SerializeField] private float oceanThreshold = 0.36f;

    [Header("Smooth Biomes")]
    public Material biomeBlendMaterial;

    [Header("Biome Materials")]
    [SerializeField] private Material grasslandMaterial;
    [SerializeField] private Material sparseForestMaterial;
    [SerializeField] private Material forestMaterial;
    [SerializeField] private Material desertMaterial;

    [Header("Ocean")]
    [SerializeField] private float seaLevel = 0f;
    [SerializeField] private Material oceanMaterial;
    [SerializeField] private float oceanSize = 2000f;

    private Transform oceanTransform;

    [Header("Player Spawn")]
    [SerializeField] private float spawnSearchRadius = 1024f;
    [SerializeField] private float spawnSearchStep = 16f;
    [SerializeField] private float playerSpawnHeight = 2f;

    [Header("World Distance")]
    [SerializeField] private int renderDistance = 8;
    [SerializeField] private int loadDistance = 10;

    [Header("Always Visible")]
    [SerializeField] private int alwaysVisibleDistance = 1;

    private Dictionary<Vector2Int, WorldChunk> loadedChunks = new Dictionary<Vector2Int, WorldChunk>();

    private Vector2Int currentPlayerChunk;
    private Plane[] cameraPlanes;

    private void Start()
    {
        if (player == null)
        {
            Debug.LogError("WorldGenerator: Player chưa được gán!");
            enabled = false;
            return;
        }

        if (chunkPrefab == null)
        {
            Debug.LogError("WorldGenerator: Chưa gán Chunk Prefab!");
            enabled = false;
            return;
        }

        if (chunkSize <= 0)
        {
            Debug.LogError("WorldGenerator: Chunk Size phải lớn hơn 0!");
            enabled = false;
            return;
        }

        if (playerCamera == null)
            playerCamera = Camera.main;

        CreateOcean();

        PlacePlayerOnLand();

        MoveOceanToPlayer();

        currentPlayerChunk = GetPlayerChunk();

        UpdateChunks();
    }

    private void Update()
    {
        MoveOceanToPlayer();

        Vector2Int playerChunk = GetPlayerChunk();

        if (playerChunk != currentPlayerChunk)
        {
            currentPlayerChunk = playerChunk;
            UpdateChunks();
        }

        UpdateChunkVisibility();
    }

    private void PlacePlayerOnLand()
    {
        Vector3 startPosition = player.position;

        float bestDistance = float.MaxValue;
        Vector3 bestPosition = startPosition;
        bool foundLand = false;

        float step = Mathf.Max(1f, spawnSearchStep);
        int steps = Mathf.CeilToInt(spawnSearchRadius / step);

        for (int x = -steps; x <= steps; x++)
        {
            for (int z = -steps; z <= steps; z++)
            {
                float worldX = startPosition.x + x * step;
                float worldZ = startPosition.z + z * step;

                float distance = x * x * step * step + z * z * step * step;

                if (distance >= bestDistance)
                    continue;

                if (GetBiome(worldX, worldZ) == BiomeType.Ocean)
                    continue;

                bestDistance = distance;

                bestPosition = new Vector3(worldX, GetTerrainHeight(worldX, worldZ) + playerSpawnHeight, worldZ);

                foundLand = true;
            }
        }

        if (foundLand)
        {
            player.position = bestPosition;
        }
        else
        {
            Debug.LogWarning("Không tìm thấy đất liền. Hãy tăng Spawn Search Radius.");
        }
    }

    private void MoveOceanToPlayer()
    {
        if (oceanTransform == null || player == null)
            return;

        oceanTransform.position = new Vector3(player.position.x, seaLevel, player.position.z);
    }

    private Vector2Int GetPlayerChunk()
    {
        int x = Mathf.FloorToInt(player.position.x / chunkSize);
        int z = Mathf.FloorToInt(player.position.z / chunkSize);

        return new Vector2Int(x, z);
    }

    private void UpdateChunks()
    {
        for (int x = -loadDistance; x <= loadDistance; x++)
        {
            for (int z = -loadDistance; z <= loadDistance; z++)
            {
                Vector2Int coordinate = currentPlayerChunk + new Vector2Int(x, z);

                LoadChunk(coordinate);
            }
        }

        UpdateChunkVisibility();
        UnloadFarChunks();
    }

    private void LoadChunk(Vector2Int coordinate)
    {
        if (loadedChunks.ContainsKey(coordinate))
            return;

        WorldChunk chunk = Instantiate(chunkPrefab, transform);

        chunk.name = "Chunk " + coordinate.x + "_" + coordinate.y;

        chunk.transform.position = new Vector3(coordinate.x * chunkSize, 0f, coordinate.y * chunkSize);

        chunk.Generate(coordinate, chunkSize, heightMultiplier, noiseScale, seed, this);

        loadedChunks.Add(coordinate, chunk);
    }

    public float GetTerrainHeight(float worldX, float worldZ)
    {
        float continent = Mathf.PerlinNoise((worldX + seed) * continentScale, (worldZ + seed) * continentScale);

        if (continent < oceanThreshold)
        {
            return seaLevel - 2f - (oceanThreshold - continent) * heightMultiplier;
        }

        float detail = Mathf.PerlinNoise((worldX + seed + 1000f) * noiseScale,(worldZ + seed + 1000f) * noiseScale);

        float mountainRegion = Mathf.PerlinNoise((worldX + seed + 2000f) * mountainRegionScale, (worldZ + seed + 2000f) * mountainRegionScale);

        float ridgeNoise = Mathf.PerlinNoise((worldX + seed + 3000f) * mountainScale, (worldZ + seed + 3000f) * mountainScale);

        float ridge = 1f - Mathf.Abs(ridgeNoise * 2f - 1f);

        float mountainMask = Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(0.55f, 0.75f, mountainRegion));

        float baseHeight = (continent - oceanThreshold) * heightMultiplier + (detail - 0.5f) * terrainDetailHeight;

        float mountains = Mathf.Pow(ridge, 2f) * mountainHeight * mountainMask;

        return Mathf.Max(seaLevel + 0.5f, baseHeight + mountains);
    }

    private Vector2 GetWarpedBiomePosition(float worldX, float worldZ)
    {
        float warpX = Mathf.PerlinNoise((worldX + seed + 7000f) * 0.0004f, (worldZ + seed + 7000f) * 0.0004f);

        float warpZ = Mathf.PerlinNoise((worldX + seed + 9000f) * 0.0004f, (worldZ + seed + 9000f) * 0.0004f);

        float warpedX = worldX + (warpX - 0.5f) * 250f;
        float warpedZ = worldZ + (warpZ - 0.5f) * 250f;

        return new Vector2(warpedX, warpedZ);
    }

    private float GetBiomeNoiseValue(float worldX, float worldZ)
    {
        Vector2 warpedPosition = GetWarpedBiomePosition(worldX, worldZ);

        return Mathf.PerlinNoise((warpedPosition.x + seed + 5000f) * biomeScale, (warpedPosition.y + seed + 5000f) * biomeScale);
    }

    public BiomeType GetBiome(float worldX, float worldZ)
    {
        float continent = Mathf.PerlinNoise((worldX + seed) * continentScale, (worldZ + seed) * continentScale);

        if (continent < oceanThreshold)
            return BiomeType.Ocean;

        float value = GetBiomeNoiseValue(worldX, worldZ);

        if (value < 0.25f)
            return BiomeType.Desert;

        if (value < 0.50f)
            return BiomeType.Grassland;

        if (value < 0.72f)
            return BiomeType.SparseForest;

        return BiomeType.Forest;
    }

    public Color GetBiomeBlendColor(float worldX, float worldZ)
    {
        float continent = Mathf.PerlinNoise((worldX + seed) * continentScale, (worldZ + seed) * continentScale);

        Color ocean = new Color(0.1f, 0.4f, 0.8f);
        Color beach = new Color(0.76f, 0.72f, 0.52f);
        Color desert = new Color(0.82f, 0.69f, 0.43f);
        Color grassland = new Color(0.36f, 0.62f, 0.24f);
        Color sparseForest = new Color(0.23f, 0.43f, 0.19f);
        Color forest = new Color(0.10f, 0.29f, 0.13f);

        if (continent < oceanThreshold + 0.025f)
        {
            float beachBlend = Mathf.InverseLerp(oceanThreshold - 0.015f, oceanThreshold + 0.025f, continent);

            return Color.Lerp(ocean, beach, Mathf.SmoothStep(0f, 1f, beachBlend));
        }

        float value = GetBiomeNoiseValue(worldX, worldZ);

        if (value < 0.25f)
        {
            float t = Mathf.InverseLerp(0.19f, 0.25f, value);

            return Color.Lerp(desert, grassland, Mathf.SmoothStep(0f, 1f, t));
        }

        if (value < 0.50f)
        {
            float t = Mathf.InverseLerp(0.44f, 0.50f, value);

            return Color.Lerp(grassland, sparseForest, Mathf.SmoothStep(0f, 1f, t));
        }

        if (value < 0.72f)
        {
            float t = Mathf.InverseLerp(0.66f, 0.72f, value);

            return Color.Lerp(sparseForest, forest, Mathf.SmoothStep(0f, 1f, t));
        }

        return forest;
    }

    public float GetResourceDensity(float worldX, float worldZ)
    {
        BiomeType biome = GetBiome(worldX, worldZ);

        switch (biome)
        {
            case BiomeType.Ocean:
            case BiomeType.Desert:
                return 0f;

            case BiomeType.Grassland:
                return 0.08f;

            case BiomeType.SparseForest:
                return 0.35f;

            case BiomeType.Forest:
                return 0.8f;

            default:
                return 0f;
        }
    }

    public Material GetBiomeMaterial(BiomeType biome)
    {
        switch (biome)
        {
            case BiomeType.Desert:
                return desertMaterial;

            case BiomeType.Grassland:
                return grasslandMaterial;

            case BiomeType.SparseForest:
                return sparseForestMaterial;

            case BiomeType.Forest:
                return forestMaterial;

            default:
                return null;
        }
    }

    public float GetSeaLevel()
    {
        return seaLevel;
    }

    private void CreateOcean()
    {
        GameObject ocean = GameObject.CreatePrimitive(PrimitiveType.Plane);

        ocean.name = "Ocean";
        ocean.transform.SetParent(transform);

        oceanTransform = ocean.transform;

        float scale = oceanSize / 10f;

        oceanTransform.localScale = new Vector3(scale, 1f, scale);

        oceanTransform.position = new Vector3(player.position.x, seaLevel, player.position.z);

        Collider oceanCollider = ocean.GetComponent<Collider>();

        if (oceanCollider != null)
            Destroy(oceanCollider);

        Renderer oceanRenderer = ocean.GetComponent<Renderer>();

        if (oceanMaterial != null)
        {
            oceanRenderer.sharedMaterial = oceanMaterial;
        }
        else
        {
            oceanRenderer.material.color = new Color(0.1f, 0.4f, 0.8f);
        }
    }

    private void UpdateChunkVisibility()
    {
        if (playerCamera == null)
            return;

        cameraPlanes = GeometryUtility.CalculateFrustumPlanes(playerCamera);

        foreach (KeyValuePair<Vector2Int, WorldChunk> pair in loadedChunks)
        {
            Vector2Int coordinate = pair.Key;
            WorldChunk chunk = pair.Value;

            if (IsInsideAlwaysVisibleDistance(coordinate))
            {
                chunk.SetVisible(true);
                continue;
            }

            if (!IsInsideRenderDistance(coordinate))
            {
                chunk.SetVisible(false);
                continue;
            }

            bool insideCamera = GeometryUtility.TestPlanesAABB(cameraPlanes, chunk.GetBounds());

            chunk.SetVisible(insideCamera);
        }
    }

    private bool IsInsideAlwaysVisibleDistance(Vector2Int coordinate)
    {
        return Mathf.Abs(coordinate.x - currentPlayerChunk.x) <= alwaysVisibleDistance && Mathf.Abs(coordinate.y - currentPlayerChunk.y) <= alwaysVisibleDistance;
    }

    private bool IsInsideRenderDistance(Vector2Int coordinate)
    {
        return Mathf.Abs(coordinate.x - currentPlayerChunk.x) <= renderDistance && Mathf.Abs(coordinate.y - currentPlayerChunk.y) <= renderDistance;
    }

    private void UnloadFarChunks()
    {
        List<Vector2Int> chunksToRemove = new List<Vector2Int>();

        foreach (KeyValuePair<Vector2Int, WorldChunk> pair in loadedChunks)
        {
            int xDistance = Mathf.Abs(pair.Key.x - currentPlayerChunk.x);
            int zDistance = Mathf.Abs(pair.Key.y - currentPlayerChunk.y);

            if (xDistance > loadDistance || zDistance > loadDistance)
            {
                Destroy(pair.Value.gameObject);
                chunksToRemove.Add(pair.Key);
            }
        }

        foreach (Vector2Int coordinate in chunksToRemove)
            loadedChunks.Remove(coordinate);
    }
}