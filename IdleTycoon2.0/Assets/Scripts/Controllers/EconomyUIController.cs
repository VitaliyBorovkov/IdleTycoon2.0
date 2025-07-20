using UnityEngine;

public class EconomyUIController : MonoBehaviour
{
    [SerializeField] private MonoBehaviour economyServiceSource;
    [SerializeField] private MoneyView moneyView;

    private IEconomyService economyService;

    public void Initialize()
    {
        economyService = economyServiceSource as IEconomyService;

        moneyView.UpdateMoney(economyService.GetMoney());
        economyService.OnMoneyChanged += moneyView.UpdateMoney;
    }

    private void OnDestroy()
    {
        economyService.OnMoneyChanged -= moneyView.UpdateMoney;
    }
}
