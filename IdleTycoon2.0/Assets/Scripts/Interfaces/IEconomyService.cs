using System;

public interface IEconomyService
{
    event Action<int> OnMoneyChanged;

    void AddMoney(int amount);
    bool TrySpendMoney(int amount);
    int GetMoney();
}