using UnityEngine;

public enum BiomeType
{
    Ocean,
    Desert,
    Grassland,
    SparseForest,
    Forest
}

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(ResourceSpawner))]
public class WorldChunk : MonoBehaviour
{
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    private ResourceSpawner resourceSpawner;

    private int chunkSize;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        resourceSpawner = GetComponent<ResourceSpawner>();
    }

    public void Generate(Vector2Int chunkCoordinate, int chunkSize, float heightMultiplier, float noiseScale, int seed, WorldGenerator worldGenerator)
    {
        this.chunkSize = chunkSize;

        GenerateMesh(chunkCoordinate, worldGenerator);

        float centerX = chunkCoordinate.x * chunkSize + chunkSize / 2f;
        float centerZ = chunkCoordinate.y * chunkSize + chunkSize / 2f;

        if (worldGenerator.biomeBlendMaterial != null)
        {
            meshRenderer.sharedMaterial = worldGenerator.biomeBlendMaterial;
        }
        else
        {
            BiomeType biome = worldGenerator.GetBiome(centerX, centerZ);

            Material biomeMaterial = worldGenerator.GetBiomeMaterial(biome);

            if (biomeMaterial != null)
                meshRenderer.sharedMaterial = biomeMaterial;
        }

        resourceSpawner.Spawn(chunkCoordinate, chunkSize, seed, worldGenerator);
    }

    private void GenerateMesh(Vector2Int chunkCoordinate, WorldGenerator worldGenerator)
    {
        int vertexCount = chunkSize + 1;
        int vertexCountTotal = vertexCount * vertexCount;

        Vector3[] vertices = new Vector3[vertexCountTotal];
        Vector2[] uvs = new Vector2[vertexCountTotal];
        Color[] colors = new Color[vertexCountTotal];
        Vector3[] normals = new Vector3[vertexCountTotal];

        int[] triangles = new int[chunkSize * chunkSize * 6];

        for (int z = 0; z <= chunkSize; z++)
        {
            for (int x = 0; x <= chunkSize; x++)
            {
                int index = z * vertexCount + x;

                float worldX = chunkCoordinate.x * chunkSize + x;
                float worldZ = chunkCoordinate.y * chunkSize + z;

                float height = worldGenerator.GetTerrainHeight(worldX, worldZ);

                vertices[index] = new Vector3(x, height, z);

                uvs[index] = new Vector2((float)x / chunkSize, (float)z / chunkSize);

                colors[index] = worldGenerator.GetBiomeBlendColor(worldX, worldZ);

                float left = worldGenerator.GetTerrainHeight(worldX - 1f, worldZ);
                float right = worldGenerator.GetTerrainHeight(worldX + 1f, worldZ);

                float back = worldGenerator.GetTerrainHeight(worldX, worldZ - 1f);
                float forward = worldGenerator.GetTerrainHeight(worldX, worldZ + 1f);

                float slopeX = (right - left) * 0.5f;
                float slopeZ = (forward - back) * 0.5f;

                normals[index] = new Vector3(-slopeX, 1f, -slopeZ).normalized;
            }
        }

        int triangleIndex = 0;

        for (int z = 0; z < chunkSize; z++)
        {
            for (int x = 0; x < chunkSize; x++)
            {
                int bottomLeft = z * vertexCount + x;
                int bottomRight = bottomLeft + 1;
                int topLeft = bottomLeft + vertexCount;
                int topRight = topLeft + 1;

                triangles[triangleIndex++] = bottomLeft;
                triangles[triangleIndex++] = topLeft;
                triangles[triangleIndex++] = topRight;

                triangles[triangleIndex++] = bottomLeft;
                triangles[triangleIndex++] = topRight;
                triangles[triangleIndex++] = bottomRight;
            }
        }

        Mesh mesh = new Mesh();
        mesh.name = "World Chunk";

        if (vertices.Length > 65535)
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.colors = colors;
        mesh.normals = normals;

        mesh.RecalculateBounds();

        meshFilter.mesh = mesh;

        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = mesh;
    }

    public Bounds GetBounds()
    {
        return meshRenderer.bounds;
    }

    public void SetVisible(bool visible)
    {
        meshRenderer.enabled = visible;
        meshCollider.enabled = visible;

        foreach (Transform child in transform)
            child.gameObject.SetActive(visible);
    }

    private void OnDrawGizmosSelected()
    {
        if (meshRenderer == null)
            meshRenderer = GetComponent<MeshRenderer>();

        if (meshRenderer == null)
            return;

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireCube(meshRenderer.bounds.center, meshRenderer.bounds.size);
    }
}