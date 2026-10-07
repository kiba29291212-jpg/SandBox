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
    [SerializeField] private int seed = 12345;

    [Header("World Distance")]
    [SerializeField] private int renderDistance = 4;
    [SerializeField] private int loadDistance = 6;

    [Header("Always Visible")]
    [SerializeField] private int alwaysVisibleDistance = 1;

    private Dictionary<Vector2Int, WorldChunk> loadedChunks =
        new Dictionary<Vector2Int, WorldChunk>();

    private Vector2Int currentPlayerChunk;

    private Plane[] cameraPlanes;

    private void Start()
    {
        Debug.Log("WORLD GENERATOR START");

        if (playerCamera == null)
            playerCamera = Camera.main;

        currentPlayerChunk = GetPlayerChunk();

        UpdateChunks();
    }

    private void Update()
    {
        Vector2Int playerChunk = GetPlayerChunk();

        if (playerChunk != currentPlayerChunk)
        {
            currentPlayerChunk = playerChunk;
            UpdateChunks();
        }

        UpdateChunkVisibility();
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
                Vector2Int chunkCoordinate =
                    currentPlayerChunk + new Vector2Int(x, z);

                LoadChunk(chunkCoordinate);
            }
        }

        UpdateChunkVisibility();
        UnloadFarChunks();
    }

    private void LoadChunk(Vector2Int chunkCoordinate)
    {
        if (loadedChunks.ContainsKey(chunkCoordinate))
            return;

        Debug.Log("LOAD CHUNK: " + chunkCoordinate);

        WorldChunk chunk = Instantiate(
            chunkPrefab,
            transform
        );

        chunk.name =
            "Chunk " +
            chunkCoordinate.x +
            "_" +
            chunkCoordinate.y;

        chunk.transform.position = new Vector3(
            chunkCoordinate.x * chunkSize,
            0f,
            chunkCoordinate.y * chunkSize
        );

        chunk.Generate(
            chunkCoordinate,
            chunkSize,
            heightMultiplier,
            noiseScale,
            seed
        );

        loadedChunks.Add(chunkCoordinate, chunk);
    }

    private void UpdateChunkVisibility()
    {
        if (playerCamera == null)
            return;

        cameraPlanes = GeometryUtility.CalculateFrustumPlanes(
            playerCamera
        );

        foreach (KeyValuePair<Vector2Int, WorldChunk> chunk in loadedChunks)
        {
            Vector2Int coordinate = chunk.Key;

            bool alwaysVisible =
                IsInsideAlwaysVisibleDistance(coordinate);

            if (alwaysVisible)
            {
                chunk.Value.SetVisible(true);
                continue;
            }

            bool insideRenderDistance =
                IsInsideRenderDistance(coordinate);

            if (!insideRenderDistance)
            {
                chunk.Value.SetVisible(false);
                continue;
            }

            bool insideCamera =
                GeometryUtility.TestPlanesAABB(
                    cameraPlanes,
                    chunk.Value.GetBounds()
                );

            chunk.Value.SetVisible(insideCamera);
        }
    }

    private bool IsInsideAlwaysVisibleDistance(
        Vector2Int chunkCoordinate)
    {
        int xDistance = Mathf.Abs(
            chunkCoordinate.x - currentPlayerChunk.x
        );

        int zDistance = Mathf.Abs(
            chunkCoordinate.y - currentPlayerChunk.y
        );

        return
            xDistance <= alwaysVisibleDistance &&
            zDistance <= alwaysVisibleDistance;
    }

    private bool IsInsideRenderDistance(
        Vector2Int chunkCoordinate)
    {
        int xDistance = Mathf.Abs(
            chunkCoordinate.x - currentPlayerChunk.x
        );

        int zDistance = Mathf.Abs(
            chunkCoordinate.y - currentPlayerChunk.y
        );

        return
            xDistance <= renderDistance &&
            zDistance <= renderDistance;
    }

    private void UnloadFarChunks()
    {
        List<Vector2Int> chunksToRemove =
            new List<Vector2Int>();

        foreach (KeyValuePair<Vector2Int, WorldChunk> chunk in loadedChunks)
        {
            int xDistance = Mathf.Abs(
                chunk.Key.x - currentPlayerChunk.x
            );

            int zDistance = Mathf.Abs(
                chunk.Key.y - currentPlayerChunk.y
            );

            bool outsideLoadDistance =
                xDistance > loadDistance ||
                zDistance > loadDistance;

            if (outsideLoadDistance)
            {
                Destroy(chunk.Value.gameObject);

                chunksToRemove.Add(chunk.Key);
            }
        }

        foreach (Vector2Int coordinate in chunksToRemove)
            loadedChunks.Remove(coordinate);
    }
}