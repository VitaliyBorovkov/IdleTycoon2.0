using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EconomyService economyService;
    [SerializeField] private InventoryService inventoryService;
    [SerializeField] private EconomyUIController economyUIController;
    [SerializeField] private MillController millController;
    [SerializeField] private PlayerLevelService playerLevelService;
    [SerializeField] private XPBarController xpBarController;
    [SerializeField] private InventoryPanelController inventoryPanelController;
    [SerializeField] private BakeryController bakeryController;
    [SerializeField] private BreadSellController breadSellController;
    [SerializeField] private MillerBotController millerBotController;
    [SerializeField] private Transform bakeryPoint;
    [SerializeField] private BakerBotController bakerBotController;
    [SerializeField] private Transform warehousePoint;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        economyService.Initialize();
        inventoryService.Initialize();
        economyUIController.Initialize();
        xpBarController.Initialize(playerLevelService);
        millController.Initialize(inventoryService);
        inventoryPanelController.Initialize();
        bakeryController.Initialize(inventoryService);
        breadSellController.Initialize(inventoryService, economyService, playerLevelService);
        millerBotController.Initialize(inventoryService, bakeryPoint, playerLevelService, economyService, bakeryController);
        bakerBotController.Initialize(inventoryService, warehousePoint, playerLevelService, economyService);
    }
}
