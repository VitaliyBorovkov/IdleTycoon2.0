using System.Collections.Generic;

using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Services")]
    [SerializeField] private EconomyService economyService;
    [SerializeField] private InventoryService inventoryService;
    [SerializeField] private PlayerLevelService playerLevelService;

    [Header("Controllers")]
    [SerializeField] private MillController millController;
    [SerializeField] private XPBarController xpBarController;
    [SerializeField] private BakeryController bakeryController;
    [SerializeField] private EconomyUIController economyUIController;
    [SerializeField] private BreadSellController breadSellController;
    [SerializeField] private InventoryPanelController inventoryPanelController;

    [Header("Bots")]
    [SerializeField] private Transform millPoint;
    [SerializeField] private Transform bakeryPoint;
    [SerializeField] private GameObject farmPrefab;
    [SerializeField] private Transform warehousePoint;
    [SerializeField] private GameObject bakerBotPrefab;
    [SerializeField] private Transform farmerSpawnPoint;
    [SerializeField] private GameObject farmerBotPrefab;
    [SerializeField] private GameObject millerBotPrefab;
    [SerializeField] private BakerBotController bakerBotController;
    [SerializeField] private MillerBotController millerBotController;

    [SerializeField] private BuildSlotController[] buildSlots;

    private SaveService saveService;
    private SaveController saveController;
    private LoadController loadController;

    private readonly List<IBuildingSave> buildingSaves = new();
    private readonly List<IBotSave> botSaves = new();

    private void Start()
    {
        Initialize();
        LoadGame();
    }

    private void Initialize()
    {
        saveService = new SaveService();
        saveController = new SaveController(saveService, playerLevelService, economyService, inventoryService,
             buildingSaves, botSaves);

        loadController = new LoadController(saveService, playerLevelService, economyService, inventoryService,
           this, buildSlots, farmPrefab, farmerBotPrefab, millerBotPrefab, bakerBotPrefab, millPoint,
            bakeryPoint, warehousePoint, farmerSpawnPoint, bakeryController);

        economyService.Initialize();
        inventoryService.Initialize();
        economyUIController.Initialize();
        inventoryPanelController.Initialize();
        millController.Initialize(inventoryService);
        bakeryController.Initialize(inventoryService);
        xpBarController.Initialize(playerLevelService);
        breadSellController.Initialize(inventoryService, economyService, playerLevelService);
        //bakerBotController.Initialize(inventoryService, warehousePoint, playerLevelService, economyService);
        //millerBotController.Initialize(inventoryService, bakeryPoint, playerLevelService, economyService, bakeryController);

        RegisterBot(millerBotController);
        RegisterBot(bakerBotController);

        Debug.Log("[GameManager] SaveController initialized");
    }

    public void SaveGame()
    {
        if (saveController == null)
        {
            Debug.LogWarning("[GameManager] SaveController is not initialized.");
            return;
        }

        saveController.SaveGame();
    }

    public void RegisterBuilding(IBuildingSave building)
    {
        if (!buildingSaves.Contains(building))
        {
            buildingSaves.Add(building);
        }
    }

    public void RegisterBot(IBotSave bot)
    {
        if (!botSaves.Contains(bot))
        {
            botSaves.Add(bot);
        }
    }

    public void LoadGame()
    {
        if (loadController == null)
        {
            Debug.LogWarning("[GameManager] LoadController is not initialized.");
            return;
        }

        loadController.LoadGame();
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("[GameManager] Saving game (Editor mode)");
            SaveGame();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadGame();
        }
    }
#endif
}
