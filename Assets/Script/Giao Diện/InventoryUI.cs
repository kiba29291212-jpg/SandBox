
using NUnit.Framework;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;

    private PlayerInputActions input;
    private bool isOpen;
    public static bool IsOpen { get; private set; }

    private void Awake()
    {
        input = new PlayerInputActions();
    }

    private void OnEnable()
    {
        input.Player.Inventory.performed += OnInventoryPerformed;
        input.Enable();
    }

    private void OnDisable()
    {
        input.Player.Inventory.performed -= OnInventoryPerformed;
        input.Disable();
    }

    private void Start()
    {
        isOpen = false;
        IsOpen = false;
        inventoryPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnInventoryPerformed(
        UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        ToggleInventory();
    }

    private void ToggleInventory()
    {
        isOpen = !isOpen;
        IsOpen = isOpen;
        inventoryPanel.SetActive(isOpen);

        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isOpen;
    }
}
