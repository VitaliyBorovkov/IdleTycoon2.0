using System.Collections;

using UnityEngine;

public class MillerBotController : BotBase, IBotSave
{
    [SerializeField] private MillerSettings settings;

    private IPlayerLevelService playerLevelService;
    private IInventoryService inventoryService;
    private IEconomyService economyService;

    private BakeryController bakeryController;
    private Transform bakeryPoint;
    private Vector3 startPoint;

    public void Initialize(IInventoryService inventoryService, Transform bakeryPoint, IPlayerLevelService playerLevelService,
        IEconomyService economyService, BakeryController bakeryController)
    {
        this.inventoryService = inventoryService;
        this.playerLevelService = playerLevelService;
        this.economyService = economyService;
        this.bakeryController = bakeryController;
        this.bakeryPoint = bakeryPoint;
        startPoint = transform.position;

        StartCoroutine(WorkLoop());
    }

    private IEnumerator WorkLoop()
    {
        while (true)
        {
            while (inventoryService.GetAmount(ItemType.Flour) < settings.flourCarryAmount)
            {
                yield return new WaitForSeconds(0.5f);
            }

            bool success = inventoryService.TryConsume(ItemType.Flour, settings.flourCarryAmount);
            if (!success)
            {
                Debug.LogWarning("[MillerBot] Failed to consume grain after check.");
                yield return null;
                continue;
            }

            Debug.Log($"[MillerBot] Picked up {settings.flourCarryAmount} flour. Moving to bakery...");

            yield return StartCoroutine(MoveTo(bakeryPoint.position, settings.moveSpeed));
            yield return new WaitForSeconds(0.5f);

            inventoryService.Add(ItemType.Flour, settings.flourCarryAmount);
            bakeryController.NotifyFlourDelivered(settings.flourCarryAmount);

            playerLevelService.AddXP(settings.xpForDelivered);
            economyService.AddMoney(settings.moneyForDelivered);

            yield return new WaitForSeconds(0.5f);
            Debug.Log($"[MillerBot] Delivered flour. XP +{settings.xpForDelivered}, Money +{settings.moneyForDelivered}");

            Debug.Log($"[MillerBot] Delivered flour to bakery. Returning...");
            yield return StartCoroutine(MoveTo(startPoint, settings.moveSpeed));
        }
    }

    public BotData GetBotData()
    {
        return new BotData
        {
            botType = "MillerBot",
            botLevel = 0,
        };
    }
}