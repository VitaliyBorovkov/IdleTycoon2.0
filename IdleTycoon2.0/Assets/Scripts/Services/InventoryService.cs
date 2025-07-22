using System;

using UnityEngine;

public class InventoryService : MonoBehaviour, IInventoryService
{
    private InventoryModel model;

    public event Action OnInventoryChanged;

    public void Initialize()
    {
        model = new InventoryModel();
    }

    public int GetAmount(ItemType type)
    {
        return model.GetAmount(type);
    }

    public void Add(ItemType type, int amount)
    {
        model.Add(type, amount);
        Debug.Log($"[Inventory] +{amount} {type} (Total: {model.GetAmount(type)})");
        OnInventoryChanged?.Invoke();
    }

    public bool TryConsume(ItemType type, int amount)
    {
        bool success = model.TryConsume(type, amount);
        if (success)
        {
            Debug.Log($"[Inventory] -{amount} {type} (Left: {model.GetAmount(type)})");
            OnInventoryChanged?.Invoke();
        }
        else
        {
            Debug.Log($"[Inventory] Not enough {type} to consume {amount}");
        }

        return success;
    }

    public void SetItem(ItemType type, int amount)
    {
        model.SetAmount(type, amount);
        Debug.Log($"[Inventory] Set {type} = {amount}");
        OnInventoryChanged?.Invoke();
    }
}