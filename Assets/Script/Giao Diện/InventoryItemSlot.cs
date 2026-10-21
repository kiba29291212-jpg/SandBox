using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventoryItemSlot : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [Header("UI")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private Image selectedBorder;

    private int slotIndex;
    private Canvas rootCanvas;

    private static int draggedSlot = -1;
    private static Image dragImage;

    private void Awake()
    {
        rootCanvas = GetComponentInParent<Canvas>();
    }

    public void SetSlot(
        int index, ItemData item, int amount, bool selected)
    {
        slotIndex = index;

        bool hasItem = item != null && amount > 0;

        if (itemIcon != null)
        {
            itemIcon.sprite = hasItem ? item.icon : null;
            itemIcon.enabled = hasItem && item.icon != null;
        }

        if (amountText != null)
        {
            amountText.text = hasItem && amount > 1 ? amount.ToString() : "";
        }

        if (selectedBorder != null)
            selectedBorder.enabled = selected;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (InventoryManager.Instance == null)
            return;

        ItemData item = InventoryManager.Instance.GetItem(slotIndex);

        if (item == null || item.icon == null || rootCanvas == null)
            return;

        draggedSlot = slotIndex;

        GameObject dragObject = new GameObject("DraggedItemIcon");
        dragObject.transform.SetParent(rootCanvas.transform, false);

        dragImage = dragObject.AddComponent<Image>();
        dragImage.sprite = item.icon;
        dragImage.preserveAspect = true;
        dragImage.raycastTarget = false;

        RectTransform rect = dragObject.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(60f, 60f);

        UpdateDragPosition(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateDragPosition(eventData);
    }

    private void UpdateDragPosition(PointerEventData eventData)
    {
        if (dragImage == null || rootCanvas == null)
            return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rootCanvas.transform as RectTransform, eventData.position, rootCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : rootCanvas.worldCamera, out Vector2 localPoint);

        dragImage.rectTransform.localPosition = localPoint;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (draggedSlot < 0 || InventoryManager.Instance == null)
            return;

        InventoryManager.Instance.MoveOrMergeSlots(draggedSlot, slotIndex);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragImage != null)
            Destroy(dragImage.gameObject);

        dragImage = null;
        draggedSlot = -1;
    }
}
