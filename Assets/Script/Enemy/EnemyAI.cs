
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("Player")]
    public Transform player;

    [Header("Detection")]
    public float detectionRange = 15f;
    public float loseTargetRange = 22f;

    [Header("Movement")]
    public float moveSpeed = 3.5f;
    public float updatePathInterval = 0.2f;

    [Header("NavMesh")]
    public float navMeshSearchRadius = 2f;

    private NavMeshAgent agent;
    private bool isChasing;
    private float pathTimer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        agent.speed = moveSpeed;

        // Khong goi isStopped neu agent chua nam tren NavMesh.
        if (agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }
    }

    private void Update()
    {
        if (agent == null || !agent.enabled ||
            !gameObject.activeInHierarchy)
            return;

        // Cho NavMesh runtime san sang.
        if (!agent.isOnNavMesh)
        {
            TryAttachToNavMesh();
            return;
        }

        if (player == null)
        {
            StopChasing();
            return;
        }

        float distance = Vector3.Distance(
            transform.position,
            player.position
        );

        if (!isChasing && distance <= detectionRange)
        {
            isChasing = true;
            pathTimer = 0f;
        }

        if (isChasing && distance >= loseTargetRange)
        {
            StopChasing();
            return;
        }

        if (isChasing)
        {
            ChasePlayer();
        }
    }

    private void TryAttachToNavMesh()
    {
        NavMeshHit hit;

        if (NavMesh.SamplePosition(
            transform.position,
            out hit,
            navMeshSearchRadius,
            agent.areaMask))
        {
            agent.Warp(hit.position);

            if (agent.isOnNavMesh)
                agent.isStopped = !isChasing;
        }
    }

    private void ChasePlayer()
    {
        if (!agent.isOnNavMesh)
            return;

        agent.isStopped = false;

        pathTimer -= Time.deltaTime;

        if (pathTimer <= 0f)
        {
            agent.SetDestination(player.position);
            pathTimer = updatePathInterval;
        }
    }

    private void StopChasing()
    {
        isChasing = false;

        if (agent != null &&
            agent.enabled &&
            agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }
    }

    private void OnDisable()
    {
        isChasing = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, loseTargetRange);
    }
}