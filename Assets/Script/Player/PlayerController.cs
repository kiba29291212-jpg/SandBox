using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -20f;

    [Header("Ground Check")]
    [SerializeField] private float groundCheckDistance = 0.3f;

    private CharacterController controller;
    private PlayerInputActions input;
    private Camera playerCamera;

    private Vector2 moveInput;
    private float verticalVelocity;

    private bool isGrounded;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        input = new PlayerInputActions();
        playerCamera = Camera.main;
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void Update()
    {
        CheckGround();
        HandleMovement();
        HandleJump();
    }

    private void CheckGround()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;

        float distance = controller.height / 2f + groundCheckDistance;

        isGrounded = Physics.SphereCast(
            origin,
            controller.radius * 0.9f,
            Vector3.down,
            out RaycastHit hit,
            distance
        );

        Debug.DrawRay(origin, Vector3.down * distance, isGrounded ? Color.green : Color.red);

        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }
    }

    private void HandleMovement()
    {
        moveInput = input.Player.Move.ReadValue<Vector2>();

        Vector3 cameraForward = playerCamera.transform.forward;
        Vector3 cameraRight = playerCamera.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 direction = cameraForward * moveInput.y + cameraRight * moveInput.x;

        float speed = input.Player.Sprint.IsPressed() ? sprintSpeed : moveSpeed;

        controller.Move(direction * speed * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (input.Player.Jump.WasPressedThisFrame() && isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        verticalVelocity += gravity * Time.deltaTime;

        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }
}