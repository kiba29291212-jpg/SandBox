using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private float lifeTime = 5f;

    private Transform target;
    private float damage;

    public void SetTarget(Transform target, float damage)
    {
        this.target = target;
        this.damage = damage;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.position - transform.position;

        transform.position +=
            direction.normalized * speed * Time.deltaTime;

        if (direction.magnitude < 0.5f)
        {
            HitPlayer();
        }
    }

    private void HitPlayer()
    {
        Player player = target.GetComponent<Player>();

        if (player != null)
        {
            player.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}