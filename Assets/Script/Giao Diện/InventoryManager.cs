using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("Inventory Slots")]
    [SerializeField] private int hotbarSize = 9;
    [SerializeField] private int backpackSize = 20;
    [SerializeField] private List<InventoryItemSlot> slotUIs = new();

    [Header("Held Item")]
    [SerializeField] private Transform handPoint;

    private ItemData[] items;
    private int[] amounts;

    private GameObject heldObject;
    private int selectedHotbarIndex;
    private PlayerInputActions input;
    public int TotalSlots => hotbarSize + backpackSize;
    public int SelectedHotbarIndex => selectedHotbarIndex;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        input = new PlayerInputActions();
        items = new ItemData[TotalSlots];
        amounts = new int[TotalSlots];
    }

    private void Start()
    {
        RefreshUI();
        SelectHotbar(0);
    }

    
    private void OnEnable()
    {
        if (input == null)
            return;

        input.Player.Hotbar.performed += OnHotbarPerformed;
        input.Player.Hotbar.Enable();
    }

    private void OnDisable()
    {
        if (input == null)
            return;

        input.Player.Hotbar.performed -= OnHotbarPerformed;
        input.Player.Hotbar.Disable();
    }

    private void OnHotbarPerformed(
        UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (InventoryUI.IsOpen)
            return;

        string key = context.control.displayName;

        if (int.TryParse(key, out int number) &&
            number >= 1 && number <= 9)
        {
            SelectHotbar(number - 1);
        }
    }

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (InventoryUI.IsOpen)
            return;

        if (Keyboard.current.digit1Key.wasPressedThisFrame) SelectHotbar(0);
        if (Keyboard.current.digit2Key.wasPressedThisFrame) SelectHotbar(1);
        if (Keyboard.current.digit3Key.wasPressedThisFrame) SelectHotbar(2);
        if (Keyboard.current.digit4Key.wasPressedThisFrame) SelectHotbar(3);
        if (Keyboard.current.digit5Key.wasPressedThisFrame) SelectHotbar(4);
        if (Keyboard.current.digit6Key.wasPressedThisFrame) SelectHotbar(5);
        if (Keyboard.current.digit7Key.wasPressedThisFrame) SelectHotbar(6);
        if (Keyboard.current.digit8Key.wasPressedThisFrame) SelectHotbar(7);
        if (Keyboard.current.digit9Key.wasPressedThisFrame) SelectHotbar(8);
    }

    public bool AddItem(ItemData item, int amount)
    {
        if (item == null || amount <= 0)
            return false;

        for (int i = 0; i < TotalSlots; i++)
        {
            if (items[i] != item || amounts[i] >= item.maxStack)
                continue;

            int added = Mathf.Min(
                item.maxStack - amounts[i], amount);

            amounts[i] += added;
            amount -= added;

            if (amount == 0)
            {
                RefreshUI();
                UpdateHeldItem();
                return true;
            }
        }

        for (int i = 0; i < TotalSlots; i++)
        {
            if (items[i] != null)
                continue;

            int added = Mathf.Min(item.maxStack, amount);

            items[i] = item;
            amounts[i] = added;
            amount -= added;

            if (amount == 0)
            {
                RefreshUI();
                UpdateHeldItem();
                return true;
            }
        }
        return false;
    }

    public bool CanAddItem(ItemData item, int amount)
    {
        if (item == null || amount <= 0)
            return false;

        int capacity = 0;

        for (int i = 0; i < TotalSlots; i++)
        {
            if (items[i] == item)
                capacity += item.maxStack - amounts[i];
            else if (items[i] == null)
                capacity += item.maxStack;
        }

        return capacity >= amount;
    }

    public bool TryAddItem(ItemData item, int amount)
    {
        if (!CanAddItem(item, amount))
            return false;

        return AddItem(item, amount);
    }

    public void SelectHotbar(int index)
    {
        if (index < 0 || index >= hotbarSize)
            return;

        selectedHotbarIndex = index;

        RefreshUI();
        UpdateHeldItem();
    }

    public void MoveOrMergeSlots(int from, int to)
    {
        if (from < 0 || from >= TotalSlots ||
            to < 0 || to >= TotalSlots || from == to)
            return;

        if (items[from] == null)
            return;

        if (items[to] == items[from] &&
            amounts[to] < items[to].maxStack)
        {
            int moved = Mathf.Min(
                amounts[from],
                items[to].maxStack - amounts[to]);

            amounts[to] += moved;
            amounts[from] -= moved;

            if (amounts[from] <= 0)
                items[from] = null;
        }
        else
        {
            ItemData tempItem = items[from];
            int tempAmount = amounts[from];

            items[from] = items[to];
            amounts[from] = amounts[to];

            items[to] = tempItem;
            amounts[to] = tempAmount;
        }

        RefreshUI();
        UpdateHeldItem();
    }

    public ItemData GetItem(int index)
    {
        if (index < 0 || index >= TotalSlots)
            return null;

        return items[index];
    }

    public int GetAmount(int index)
    {
        if (index < 0 || index >= TotalSlots)
            return 0;

        return amounts[index];
    }

    private void RefreshUI()
    {
        for (int i = 0; i < slotUIs.Count; i++)
        {
            if (slotUIs[i] == null)
                continue;

            if (i < TotalSlots)
                slotUIs[i].SetSlot(i, items[i], amounts[i], i == selectedHotbarIndex);
        }
    }

    private void UpdateHeldItem()
    {
        if (heldObject != null)
            Destroy(heldObject);

        heldObject = null;

        if (handPoint == null)
            return;

        ItemData item = items[selectedHotbarIndex];

        if (item == null || item.heldPrefab == null)
            return;

        heldObject = Instantiate(item.heldPrefab, handPoint);
        heldObject.transform.localPosition = Vector3.zero;
        heldObject.transform.localRotation = Quaternion.identity;

        foreach (Collider col in heldObject.GetComponentsInChildren<Collider>())
            col.enabled = false;

        foreach (Rigidbody rb in heldObject.GetComponentsInChildren<Rigidbody>())
            rb.isKinematic = true;
    }
}
