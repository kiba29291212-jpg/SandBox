using UnityEngine;
using UnityEngine.InputSystem;

public class Pickup : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private int amount = 1;

    public ItemData ItemData => itemData;
    public int Amount => amount;

    public bool TryCollect()
    {
        if (InventoryManager.Instance == null)
            return false;

        bool added = InventoryManager.Instance.TryAddItem(itemData, amount);

        if (!added)
            return false;

        Destroy(gameObject);
        return true;
    }
}
