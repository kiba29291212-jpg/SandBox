using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private WorldGenerator worldGenerator;
    [SerializeField] private DayNight dayNightCycle;

    [Header("Monsters by Biome")]
    [SerializeField] private GameObject[] desertMonsters;
    [SerializeField] private GameObject[] grasslandMonsters;
    [SerializeField] private GameObject[] sparseForestMonsters;
    [SerializeField] private GameObject[] forestMonsters;

    [Header("Spawn Position")]
    [SerializeField] private float minSpawnDistance = 20f;
    [SerializeField] private float maxSpawnDistance = 35f;
    [SerializeField] private float spawnYOffset = 0f;
    [SerializeField] private int positionAttempts = 15;
    [SerializeField] private LayerMask groundLayer = ~0;
    [SerializeField] private float raycastStartHeight = 10f;

    [Header("Difficulty")]
    [SerializeField] private int baseMaxMonsters = 5;
    [SerializeField] private int extraMonstersPerDay = 2;
    [SerializeField] private float baseSpawnInterval = 12f;
    [SerializeField] private float minimumSpawnInterval = 3f;
    [SerializeField] private float intervalReductionPerDay = 1f;

    private readonly List<GameObject> aliveMonsters = new List<GameObject>();

    private float spawnTimer;

    private void Update()
    {
        if (player == null || worldGenerator == null || dayNightCycle == null)
            return;

        aliveMonsters.RemoveAll(monster => monster == null);

        if (!IsNight())
        {
            spawnTimer = 0f;
            return;
        }

        int day = dayNightCycle.DayCount;

        int maxMonsters = baseMaxMonsters + (day - 1) * extraMonstersPerDay;

        if (aliveMonsters.Count >= maxMonsters)
            return;

        float interval = Mathf.Max(minimumSpawnInterval, baseSpawnInterval - (day - 1) * intervalReductionPerDay);

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= interval)
        {
            spawnTimer = 0f;
            SpawnMonster();
        }
    }

    private bool IsNight()
    {
        float time = dayNightCycle.TimeOfDay;

        return time >= 18f || time < 6f;
    }

    
    private void SpawnMonster()
    {
        for (int i = 0; i < positionAttempts; i++)
        {
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float distance = Random.Range(minSpawnDistance, maxSpawnDistance);

            float x = player.position.x + Mathf.Cos(angle) * distance;
            float z = player.position.z + Mathf.Sin(angle) * distance;

            BiomeType biome = worldGenerator.GetBiome(x, z);

            if (biome == BiomeType.Ocean)
                continue;

            GameObject[] monsters = GetMonstersForBiome(biome);

            if (monsters == null || monsters.Length == 0)
                continue;

            GameObject prefab = monsters[Random.Range(0, monsters.Length)];

            if (prefab == null)
                continue;

            float terrainHeight = worldGenerator.GetTerrainHeight(x, z);

            if (terrainHeight <= worldGenerator.GetSeaLevel())
                continue;

            Vector3 rayOrigin = new Vector3(x, terrainHeight + 10f, z);

            if (!Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 40f, groundLayer, QueryTriggerInteraction.Ignore))
            {
                continue;
            }

            Vector3 spawnPosition = hit.point + Vector3.up * spawnYOffset;

            GameObject monster = Instantiate(prefab, spawnPosition, Quaternion.Euler( 0f, Random.Range(0f, 360f), 0f));

            aliveMonsters.Add(monster);
            return;
        }
    }


    private GameObject[] GetMonstersForBiome(BiomeType biome)
    {
        switch (biome)
        {
            case BiomeType.Desert:
                return desertMonsters;

            case BiomeType.Grassland:
                return grasslandMonsters;

            case BiomeType.SparseForest:
                return sparseForestMonsters;

            case BiomeType.Forest:
                return forestMonsters;

            default:
                return null;
        }
    }
}
