using UnityEngine;
using UnityEngine.InputSystem;

public class CollectItem : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float pickupDistance = 3f;

    private PlayerInputActions input;

    private void Awake()
    {
        input = new PlayerInputActions();

        if (playerCamera == null)
            playerCamera = Camera.main;
    }

    private void OnEnable()
    {
        input.Player.Interact.performed += OnInteract;
        input.Enable();
    }

    private void OnDisable()
    {
        input.Player.Interact.performed -= OnInteract;
        input.Disable();
    }

    private void OnInteract(
        InputAction.CallbackContext context)
    {
        if (InventoryUI.IsOpen)
            return;

        if (playerCamera == null)
            return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, pickupDistance))
        {
            Pickup resource = hit.collider.GetComponentInParent<Pickup>();

            if (resource != null)
                resource.TryCollect();
        }
    }
}
