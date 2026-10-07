using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [Header("Mouse Settings")]
    [SerializeField] private float mouseSensitivity = 30f;

    [Header("Camera Limits")]
    [SerializeField] private float minLookAngle = -90f;
    [SerializeField] private float maxLookAngle = 90f;

    private PlayerInputActions input;

    private float xRotation = 0f;

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
        input.Disable();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        Look();
    }

    private void Look()
{
    Vector2 lookInput = input.Player.Look.ReadValue<Vector2>();

    float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
    float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

    xRotation -= mouseY;
    xRotation = Mathf.Clamp(xRotation, minLookAngle, maxLookAngle);

    transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

    transform.parent.Rotate(Vector3.up * mouseX);
}
}