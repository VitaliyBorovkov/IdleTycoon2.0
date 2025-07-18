using System;

using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private InventoryModel model;

    public event Action<ItemType, int> OnInventoryChanged;

    public void Initialize()
    {
        model = new InventoryModel();
        Debug.Log("[Inventory] Initialized");
    }

    public int GetAmount(ItemType type)
    {
        return model.GetAmount(type);
    }

    public void Add(ItemType type, int amount)
    {
        model.Add(type, amount);
        Debug.Log($"[Inventory] +{amount} {type} (Total: {model.GetAmount(type)})");
        OnInventoryChanged?.Invoke(type, model.GetAmount(type));
    }

    public bool TryConsume(ItemType type, int amount)
    {
        bool success = model.TryConsume(type, amount);
        if (success)
        {
            Debug.Log($"[Inventory] -{amount} {type} (Left: {model.GetAmount(type)})");
            OnInventoryChanged?.Invoke(type, model.GetAmount(type));
        }
        else
        {
            Debug.Log($"[Inventory] Not enough {type} to consume {amount}");
        }

        return success;
    }
}