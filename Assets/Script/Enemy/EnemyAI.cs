using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float moveSpeed = 3.5f;

    [Header("Combat Settings")]
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackDamage = 10f;

    private float currentHealth;
    private Transform player;

    private void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            MoveTowardsPlayer();

            if (distanceToPlayer <= attackRange)
            {
                AttackPlayer();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void AttackPlayer()
    {
        Player playerScript = player.GetComponent<Player>();
        if (playerScript != null)
        {
            playerScript.TakeDamage(attackDamage);
        }
    }
}