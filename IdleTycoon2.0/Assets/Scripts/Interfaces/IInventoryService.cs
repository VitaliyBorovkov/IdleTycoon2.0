using System;

public interface IInventoryService
{
    event Action OnInventoryChanged;

    int GetAmount(ItemType type);
    void Add(ItemType type, int amount);
    bool TryConsume(ItemType type, int amount);
}