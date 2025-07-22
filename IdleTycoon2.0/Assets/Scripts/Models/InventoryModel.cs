using System.Collections.Generic;

public class InventoryModel
{
    private Dictionary<ItemType, int> storage = new();

    public int GetAmount(ItemType type)
    {
        return storage.TryGetValue(type, out var amount) ? amount : 0;
    }

    public void Add(ItemType type, int amount)
    {
        if (!storage.ContainsKey(type))
            storage[type] = 0;

        storage[type] += amount;
    }

    public bool TryConsume(ItemType type, int amount)
    {
        if (GetAmount(type) < amount) return false;

        storage[type] -= amount;
        return true;
    }

    public void SetAmount(ItemType type, int amount)
    {
        storage[type] = amount;
    }
}