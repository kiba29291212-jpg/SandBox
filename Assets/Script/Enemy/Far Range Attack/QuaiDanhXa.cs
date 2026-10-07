using UnityEngine;

public class QuaiDanhXa : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private float attackDamage = 15f;
    [SerializeField] private float attackRange = 15f;
    [SerializeField] private float attackCooldown = 2f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float preferredDistance = 8f;

    [Header("Arrow")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform shootPoint;

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

        FacePlayer();

        if (distance < preferredDistance)
        {
            MoveAwayFromPlayer();
        }
        else if (distance <= attackRange)
        {
            Attack();
        }
    }

    private void FacePlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.magnitude > 0.1f)
        {
            transform.rotation =
                Quaternion.LookRotation(direction);
        }
    }

    private void MoveAwayFromPlayer()
    {
        Vector3 direction = transform.position - player.position;
        direction.y = 0f;

        if (direction.magnitude > 0.1f)
        {
            direction.Normalize();

            transform.position +=
                direction * moveSpeed * Time.deltaTime;
        }
    }

    private void Attack()
    {
        if (attackTimer > 0f)
            return;

        attackTimer = attackCooldown;

        ShootArrow();
    }

    private void ShootArrow()
    {
        if (arrowPrefab == null || shootPoint == null)
        {
            Debug.LogError("ARROW PREFAB HOAC SHOOT POINT CHUA DUOC GAN!");
            return;
        }

        GameObject arrow = Instantiate(
            arrowPrefab,
            shootPoint.position,
            shootPoint.rotation
        );

        Arrow arrowScript = arrow.GetComponent<Arrow>();

        if (arrowScript != null)
        {
            arrowScript.SetTarget(
                player,
                attackDamage
            );
        }
    }
}