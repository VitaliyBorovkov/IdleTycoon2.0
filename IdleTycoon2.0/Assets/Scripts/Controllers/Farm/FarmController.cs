using UnityEngine;


public class FarmController : MonoBehaviour
{
    [SerializeField] private FarmView view;
    [SerializeField] private GameObject farmerBotPrefab;
    [SerializeField] private FarmUpgradeView upgradeView;

    private FarmSettings farmSettings;
    private IPlayerLevelService playerLevelService;
    private IInventoryService inventoryService;
    private IEconomyService economyService;


    private FarmModel model;
    private FarmerBotController farmerBot;

    public FarmSettings GetSettings() => farmSettings;

    public void Initialize(Transform millPoint, IPlayerLevelService playerLevelService, IInventoryService inventoryService,
        IEconomyService economyService, FarmSettings farmSettings)
    {
        this.playerLevelService = playerLevelService;
        this.inventoryService = inventoryService;
        this.economyService = economyService;
        this.farmSettings = farmSettings;

        if (playerLevelService.CurrentLevel < farmSettings.RequiredPlayerLevelForBuild)
        {
            Debug.LogWarning($"Player level too low to build this farm. Required: {farmSettings.RequiredPlayerLevelForBuild}");
            return;
        }


        //if (!economyService.TrySpendMoney(farmSettings.BuildCost))
        //{
        //    Debug.LogWarning("Not enough money to build farm");
        //    return;
        //}

        model = new FarmModel();
        SpawnFarmerBot(millPoint);

        upgradeView.Initialize();
        upgradeView.OnUpgradeClicked += HandleUpgradeClick;
        UpdateUpgradeUI();

        playerLevelService.OnLevelUp += _ => UpdateUpgradeUI();
        economyService.OnMoneyChanged += _ => UpdateUpgradeUI();
    }

    private void SpawnFarmerBot(Transform millPoint)
    {
        if (farmerBotPrefab == null)
        {
            Debug.LogError("FarmerBot prefab is not assigned!");
            return;
        }

        GameObject botGO = Instantiate(farmerBotPrefab, view.GetBotSpawnPoint().position, Quaternion.identity);
        farmerBot = botGO.GetComponent<FarmerBotController>();
        farmerBot.Initialize(millPoint, model, playerLevelService, inventoryService, economyService);
    }

    private void UpdateUpgradeUI()
    {
        if (model.Level >= farmSettings.MaxFarmLevel)
        {
            upgradeView.SetButtonVisible(false);
            return;
        }

        var next = GetNextLevelSettings();

        bool levelEnough = playerLevelService.CurrentLevel >= next.requiredPlayerLevel;
        bool moneyEnough = economyService.GetMoney() >= next.upgradeCost;

        bool showButton = levelEnough /*|| moneyEnough*/;
        bool enableButton = levelEnough && moneyEnough;

        upgradeView.SetButtonVisible(showButton);
        upgradeView.SetButtonInteractable(enableButton);
        upgradeView.SetCost(next.upgradeCost);
    }


    private void HandleUpgradeClick()
    {
        if (model.Level >= farmSettings.MaxFarmLevel)
        {
            Debug.Log("[Farm] Max level reached.");
            return;
        }

        var next = GetNextLevelSettings();

        if (playerLevelService.CurrentLevel < next.requiredPlayerLevel)
        {
            Debug.Log("[Farm] Player level too low to upgrade.");
            return;
        }

        if (!economyService.TrySpendMoney(next.upgradeCost))
        {
            Debug.Log("[Farm] Not enough money to upgrade.");
            return;
        }

        if (model.Level >= farmSettings.MaxFarmLevel)
        {
            Debug.Log("[Farm] Farm is already at max level.");
            return;
        }

        model.Upgrade();
        farmerBot.UpgradeStats(model.Level);

        Debug.Log($"[Farm] Farm upgraded to level {model.Level}");

        UpdateUpgradeUI();
    }

    private FarmLevelSettings GetNextLevelSettings()
    {
        return farmSettings.GetNextLevelSettings(model.Level);
    }

}