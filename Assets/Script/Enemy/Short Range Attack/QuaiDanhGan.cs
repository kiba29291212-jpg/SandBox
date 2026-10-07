using UnityEngine;

public class QuaiDanhGan : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 1f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2.5f;

    private Transform player;
    private Player playerHealth;

    private float attackTimer;

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject == null)
        {
            Debug.LogError("KHONG TIM THAY PLAYER!");
            return;
        }

        player = playerObject.transform;
        playerHealth = playerObject.GetComponent<Player>();
    }

    private void Update()
    {
        if (player == null || playerHealth == null)
            return;

        attackTimer -= Time.deltaTime;

        float distance = Vector3.Distance(
            transform.position,
            player.position
        );

        if (distance > attackRange)
        {
            MoveToPlayer();
        }
        else
        {
            Attack();
        }
    }

    private void MoveToPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.magnitude > 0.1f)
        {
            direction.Normalize();

            transform.position +=
                direction * moveSpeed * Time.deltaTime;

            transform.rotation =
                Quaternion.LookRotation(direction);
        }
    }

    private void Attack()
    {
        if (attackTimer > 0f)
            return;

        attackTimer = attackCooldown;

        playerHealth.TakeDamage(attackDamage);

        Debug.Log("Zombie attacked Player!");
    }
}