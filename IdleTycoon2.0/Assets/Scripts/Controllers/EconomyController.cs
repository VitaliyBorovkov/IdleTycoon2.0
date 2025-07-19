
using System;

using UnityEngine;

public class EconomyController : MonoBehaviour
{
    private EconomyModel model;


    public event Action<int> OnMoneyChanged;

    public void Initialize()
    {
        model = new EconomyModel(startMoney: 1000);
        Debug.Log($"[Economy] Game started with ${model.Money}");

        OnMoneyChanged?.Invoke(model.Money);

    }

    public void AddMoney(int amount)
    {
        model.Money += amount;
        Debug.Log($"[Economy] Money added: +{amount}, total = {model.Money}");

        OnMoneyChanged?.Invoke(model.Money);

    }

    public bool TrySpendMoney(int amount)
    {
        if (model.Money >= amount)
        {
            model.Money -= amount;
            Debug.Log($"[Economy] Money spent: -{amount}, total = {model.Money}");

            OnMoneyChanged?.Invoke(model.Money);

            return true;
        }

        Debug.Log($"[Economy] Not enough money to spend {amount}");
        return false;
    }

    public int GetMoney() => model.Money;

}

