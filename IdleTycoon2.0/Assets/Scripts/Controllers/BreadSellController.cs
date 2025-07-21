using System.Collections;

using UnityEngine;

public class BreadSellController : MonoBehaviour
{
    [SerializeField] private BreadSellSettings settings;

    private IInventoryService inventoryService;
    private IEconomyService economyService;
    private IPlayerLevelService playerLevelService;

    public void Initialize(IInventoryService inventoryService, IEconomyService economyService, IPlayerLevelService playerLevelService)
    {
        this.inventoryService = inventoryService;
        this.economyService = economyService;
        this.playerLevelService = playerLevelService;

        StartCoroutine(SaleLoop());
    }

    private IEnumerator SaleLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(settings.interval);

            if (inventoryService.GetAmount(ItemType.Bread) >= settings.breadPerSale)
            {
                inventoryService.TryConsume(ItemType.Bread, settings.breadPerSale);
                economyService.AddMoney(settings.breadPerSale * settings.moneyPerBread);
                playerLevelService.AddXP(settings.breadPerSale * settings.xpPerBread);

                Debug.Log($"[Sale] Sold {settings.breadPerSale} bread for {settings.breadPerSale * settings.moneyPerBread} (+{settings.breadPerSale * settings.xpPerBread} XP)");
            }
        }
    }
}