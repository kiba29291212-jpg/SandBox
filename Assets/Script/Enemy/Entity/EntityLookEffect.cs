using UnityEngine;

public class EntityLookEffect : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private EntityGlasses glasses;
    [SerializeField] private EntityPlayerHealth playerHealth;
    [SerializeField] private float lookRange = 300f;
    [SerializeField] private LayerMask entityLayer = ~0;

    private PlayerCamera cameraController;
    private EntityBoss currentTarget;

    public bool IsLookingAtEntity { get; private set; }

    private void Awake()
    {
        if (playerCamera == null)
            playerCamera = GetComponent<Camera>();

        if (playerCamera == null)
            playerCamera = Camera.main;

        cameraController = GetComponent<PlayerCamera>();

        if (cameraController == null && playerCamera != null)
            cameraController = playerCamera.GetComponent<PlayerCamera>();

        if (playerHealth == null)
            playerHealth = GetComponentInParent<EntityPlayerHealth>();

        if (glasses == null)
            glasses = GetComponentInParent<EntityGlasses>();
    }

    private void Update()
    {
        IsLookingAtEntity = false;
        currentTarget = null;

        if (playerCamera == null || playerHealth == null || playerHealth.IsDead)
        {
            if (cameraController != null)
                cameraController.SetEntityLookTarget(null);

            return;
        }

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (Physics.Raycast(ray, out RaycastHit hit, lookRange, entityLayer,
            QueryTriggerInteraction.Ignore))
        {
            EntityBoss boss = hit.collider.GetComponentInParent<EntityBoss>();

            if (boss != null && !boss.IsDead)
            {
                currentTarget = boss;
                IsLookingAtEntity = true;

                bool hasGlasses = glasses != null && glasses.IsEquipped;
                boss.SetGlassesState(hasGlasses);

                if (!hasGlasses)
                {
                    if (cameraController != null)
                        cameraController.SetEntityLookTarget(boss.transform);

                    playerHealth.TakeDamage(boss.LookDamage * Time.deltaTime);
                    boss.AddCorruption(Time.deltaTime);
                    return;
                }
            }
        }

        if (cameraController != null)
            cameraController.SetEntityLookTarget(null);
    }
}