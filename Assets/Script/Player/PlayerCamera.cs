using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [Header("Mouse Settings")]
    [SerializeField] private float mouseSensitivity = 30f;

    [Header("Camera Limits")]
    [SerializeField] private float minLookAngle = -90f;
    [SerializeField] private float maxLookAngle = 90f;

    [Header("Entity Look")]
    [SerializeField] private float entityPullSpeed = 2f;

    private PlayerInputActions input;
    private float xRotation;
    private Transform entityTarget;

    private void Awake()
    {
        input = new PlayerInputActions();
    }

    private void OnEnable()
    {
        input.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        if (input != null)
            input.Disable();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        if (InventoryUI.IsOpen)
            return;

        if (entityTarget != null)
        {
            PullCamera();
            return;
        }

        Look();
    }

    public void SetEntityLookTarget(Transform target)
    {
        entityTarget = target;
    }

    private void PullCamera()
    {
        Vector3 direction = entityTarget.position - transform.position;

        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            entityPullSpeed * Time.deltaTime
        );
    }

    private void Look()
    {
        Vector2 lookInput = input.Player.Look.ReadValue<Vector2>();

        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minLookAngle, maxLookAngle);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        if (transform.parent != null)
            transform.parent.Rotate(Vector3.up * mouseX);
    }
}