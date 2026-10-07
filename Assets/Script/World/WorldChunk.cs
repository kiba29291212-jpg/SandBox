using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(ResourceSpawner))]
public class WorldChunk : MonoBehaviour
{
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;
    private ResourceSpawner resourceSpawner;

    private int chunkSize;
    private float heightMultiplier;
    private float noiseScale;
    private int seed;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        resourceSpawner = GetComponent<ResourceSpawner>();

        Debug.Log("WorldChunk Awake: " + gameObject.name);
    }

   public void Generate(Vector2Int chunkCoordinate, int chunkSize, float heightMultiplier, float noiseScale, int seed)
{
    this.chunkSize = chunkSize;
    this.heightMultiplier = heightMultiplier;
    this.noiseScale = noiseScale;
    this.seed = seed;

    Debug.Log("WorldChunk Generate: " + gameObject.name);

    GenerateMesh(chunkCoordinate);

    Debug.Log("WorldChunk Mesh Generated");

    Debug.Log("ResourceSpawner reference = " + resourceSpawner);

    if (resourceSpawner == null)
    {
        Debug.LogError("RESOURCE SPAWNER IS NULL!");
        return;
    }

    Debug.Log("CALLING RESOURCE SPAWNER...");

    resourceSpawner.Spawn(chunkCoordinate, chunkSize, seed);

    Debug.Log("RESOURCE SPAWNER FINISHED!");
}
    private void GenerateMesh(Vector2Int chunkCoordinate)
    {
        int vertexCount = chunkSize + 1;

        Vector3[] vertices = new Vector3[vertexCount * vertexCount];
        Vector2[] uvs = new Vector2[vertices.Length];
        int[] triangles = new int[chunkSize * chunkSize * 6];

        for (int z = 0; z <= chunkSize; z++)
        {
            for (int x = 0; x <= chunkSize; x++)
            {
                int index = z * vertexCount + x;

                float worldX = chunkCoordinate.x * chunkSize + x;
                float worldZ = chunkCoordinate.y * chunkSize + z;

                float height = Mathf.PerlinNoise(
                    (worldX + seed) * noiseScale,
                    (worldZ + seed) * noiseScale
                );

                height *= heightMultiplier;

                vertices[index] = new Vector3(x, height, z);
                uvs[index] = new Vector2(
                    (float)x / chunkSize,
                    (float)z / chunkSize
                );
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

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshFilter.mesh = mesh;

        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = mesh;
    }

    public void SetVisible(bool visible)
    {
        GetComponent<MeshRenderer>().enabled = visible;
        meshCollider.enabled = visible;

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(visible);
        }
    }
}