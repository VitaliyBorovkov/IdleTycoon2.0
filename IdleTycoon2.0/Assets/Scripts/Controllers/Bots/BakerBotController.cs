using System.Collections;

using UnityEngine;

public class BakerBotController : BotBase, IBotSave
{
    [SerializeField] private BakerSettings settings;

    private IInventoryService inventoryService;
    private IPlayerLevelService playerLevelService;
    private IEconomyService economyService;
    private Transform warehousePoint;

    private Vector3 startPoint;

    public void Initialize(IInventoryService inventoryService, Transform warehousePoint, IPlayerLevelService playerLevelService,
        IEconomyService economyService)
    {
        this.inventoryService = inventoryService;
        this.playerLevelService = playerLevelService;
        this.economyService = economyService;
        this.warehousePoint = warehousePoint;
        startPoint = transform.position;

        StartCoroutine(WorkLoop());
    }

    private IEnumerator WorkLoop()
    {
        while (true)
        {
            while (inventoryService.GetAmount(ItemType.Bread) < settings.breadCarryAmount)
            {
                yield return new WaitForSeconds(0.5f);
            }

            bool success = inventoryService.TryConsume(ItemType.Bread, settings.breadCarryAmount);
            if (!success)
            {
                Debug.LogWarning("[BakerBot] Failed to consume flour after check.");
                continue;
            }

            Debug.Log($"[BakerBot] Carrying {settings.breadCarryAmount} bread to warehouse...");

            yield return StartCoroutine(MoveTo(warehousePoint.position, settings.moveSpeed));
            yield return new WaitForSeconds(0.5f);

            playerLevelService.AddXP(settings.xpForDelivered);
            economyService.AddMoney(settings.moneyForDelivered);

            Debug.Log($"[BakerBot] Delivered bread. Gained {settings.moneyForDelivered} and {settings.xpForDelivered} XP.");

            yield return StartCoroutine(MoveTo(startPoint, settings.moveSpeed));
        }
    }

    public BotData GetBotData()
    {
        return new BotData
        {
            botType = "BakerBot",
            botLevel = 0,
        };
    }
}