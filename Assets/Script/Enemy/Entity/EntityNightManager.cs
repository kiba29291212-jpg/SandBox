using UnityEngine;

public class EntityNightManager : MonoBehaviour
{
    [SerializeField] private DayNight dayNight;
    [SerializeField] private EntityBoss bossPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnDistance = 150f;
    [SerializeField] private float spawnHeight = 2f;
    [SerializeField] private LayerMask groundLayer = ~0;

    private EntityBoss activeBoss;
    private bool previousEntityNight;

    private void Awake()
    {
        if (dayNight == null)
            dayNight = FindFirstObjectByType<DayNight>();
    }

    private void Update()
    {
        if (dayNight == null || bossPrefab == null)
            return;

        bool entityNight = dayNight.IsEntityNight;

        if (entityNight && !previousEntityNight)
            SpawnBoss();

        if (!entityNight && previousEntityNight)
            DespawnBoss();

        previousEntityNight = entityNight;
    }

    private void SpawnBoss()
    {
        if (activeBoss != null)
            return;

        Vector3 position;

        if (spawnPoint != null)
        {
            position = spawnPoint.position;
        }
        else
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player == null)
                return;

            Vector3 direction = Random.insideUnitSphere;
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.01f)
                direction = Vector3.forward;

            direction.Normalize();

            Vector3 origin = player.transform.position + direction * spawnDistance;
            origin.y += 500f;

            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit,
                1000f, groundLayer, QueryTriggerInteraction.Ignore))
            {
                position = hit.point + Vector3.up * spawnHeight;
            }
            else
            {
                position = player.transform.position
                    + direction * spawnDistance
                    + Vector3.up * spawnHeight;
            }
        }

        activeBoss = Instantiate(
            bossPrefab,
            position,
            Quaternion.identity
        );

        activeBoss.SetInvulnerable(false);
    }

    private void DespawnBoss()
    {
        if (activeBoss == null)
            return;

        Destroy(activeBoss.gameObject);
        activeBoss = null;
    }
}