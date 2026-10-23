using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float groundCheckHeight = 5f;
    [SerializeField] private float groundCheckDistance = 15f;
    [SerializeField] private float groundOffset = 0f;
    [SerializeField] private LayerMask groundLayer = ~0;

    [Header("Combat Settings")]
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 1.5f;

    private float currentHealth;
    private float attackTimer;
    private Transform player;

    private void Start()
    {
        currentHealth = maxHealth;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            return;
        }

        SnapToGround();
    }

    private void Update()
    {
        if (player == null)
            return;

        attackTimer -= Time.deltaTime;

        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0f;

        float distance = toPlayer.magnitude;

        if (distance <= detectionRange)
        {
            if (distance > attackRange)
            {
                MoveTowardsPlayer(toPlayer.normalized);
            }
            else
            {
                if (attackTimer <= 0f)
                {
                    AttackPlayer();
                    attackTimer = attackCooldown;
                }
            }
        }
        SnapToGround();
    }

    private void MoveTowardsPlayer(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.001f)
            return;

        transform.position +=
            direction * moveSpeed * Time.deltaTime;

        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void SnapToGround()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * groundCheckHeight;

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, groundCheckHeight + groundCheckDistance, groundLayer, QueryTriggerInteraction.Ignore))
        {
            Vector3 position = transform.position;
            position.y = hit.point.y + groundOffset;
            transform.position = position;
        }
    }

    private void AttackPlayer()
    {
        Player playerScript = player.GetComponent<Player>();

        if (playerScript != null)
        {
            playerScript.TakeDamage(attackDamage);
        }
    }

    public void EnemyTakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}