using UnityEngine;

public class EconomyUIController : MonoBehaviour
{
    [SerializeField] private EconomyController economy;
    [SerializeField] private MoneyView moneyView;

    public void Initialize()
    {
        moneyView.UpdateMoney(economy.GetMoney());
        economy.OnMoneyChanged += moneyView.UpdateMoney;
    }

    private void OnDestroy()
    {
        economy.OnMoneyChanged -= moneyView.UpdateMoney;
    }
}
