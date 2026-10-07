using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player;

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

    private Dictionary<Vector2Int, WorldChunk> loadedChunks = new Dictionary<Vector2Int, WorldChunk>();

    private Vector2Int currentPlayerChunk;

    private void Start()
    {
        Debug.Log("WORLD GENERATOR START");

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
                Vector2Int chunkCoordinate = currentPlayerChunk + new Vector2Int(x, z);

                float distance = Vector2Int.Distance(currentPlayerChunk, chunkCoordinate);

                if (distance <= loadDistance)
                {
                    LoadChunk(chunkCoordinate);
                }
            }
        }

        UpdateChunkVisibility();
        UnloadFarChunks();
    }

   private void LoadChunk(Vector2Int chunkCoordinate)
{
    Debug.Log("LOAD CHUNK: " + chunkCoordinate);

    if (loadedChunks.ContainsKey(chunkCoordinate))
    {
        Debug.Log("CHUNK ALREADY EXISTS: " + chunkCoordinate);
        return;
    }

    WorldChunk chunk = Instantiate(chunkPrefab, transform);

    Debug.Log("CHUNK INSTANTIATED: " + chunk.name);

    chunk.transform.position = new Vector3(
        chunkCoordinate.x * chunkSize,
        0f,
        chunkCoordinate.y * chunkSize
    );

    Debug.Log("CALLING CHUNK GENERATE: " + chunkCoordinate);

    chunk.Generate(
        chunkCoordinate,
        chunkSize,
        heightMultiplier,
        noiseScale,
        seed
    );

    Debug.Log("CHUNK GENERATE FINISHED: " + chunkCoordinate);

    loadedChunks.Add(chunkCoordinate, chunk);
}
    private void UpdateChunkVisibility()
    {
        foreach (KeyValuePair<Vector2Int, WorldChunk> chunk in loadedChunks)
        {
            float distance = Vector2Int.Distance(currentPlayerChunk, chunk.Key);

            bool visible = distance <= renderDistance;

            chunk.Value.SetVisible(visible);
        }
    }

    private void UnloadFarChunks()
    {
        List<Vector2Int> chunksToRemove = new List<Vector2Int>();

        foreach (KeyValuePair<Vector2Int, WorldChunk> chunk in loadedChunks)
        {
            float distance = Vector2Int.Distance(currentPlayerChunk, chunk.Key);

            if (distance > loadDistance)
            {
                Destroy(chunk.Value.gameObject);
                chunksToRemove.Add(chunk.Key);
            }
        }

        foreach (Vector2Int coordinate in chunksToRemove)
        {
            loadedChunks.Remove(coordinate);
        }
    }
}