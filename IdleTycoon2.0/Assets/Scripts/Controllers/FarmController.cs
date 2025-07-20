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
    public int GetUpgradeCost() => farmSettings.UpgradeCost;
    public int GetRequiredPlayerLevelForUpgrade() => farmSettings.RequiredPlayerLevelForUpgrade;

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


        if (!economyService.TrySpendMoney(farmSettings.BuildCost))
        {
            Debug.LogWarning("Not enough money to build farm");
            return;
        }

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
        farmerBot.Initialize(millPoint, model, playerLevelService, inventoryService);
    }

    //private void UpdateUpgradeUI()
    //{
    //    bool canUpgrade =
    //        model.Level < farmSettings.MaxFarmLevel &&
    //        playerLevelService.CurrentLevel >= farmSettings.RequiredPlayerLevelForUpgrade;

    //    upgradeView.SetButtonVisible(canUpgrade);
    //    upgradeView.SetButtonInteractable(canUpgrade);
    //    upgradeView.SetCost(farmSettings.UpgradeCost);
    //}

    private void UpdateUpgradeUI()
    {
        bool levelEnough = playerLevelService.CurrentLevel >= farmSettings.RequiredPlayerLevelForUpgrade;
        bool moneyEnough = economyService.GetMoney() >= farmSettings.UpgradeCost;
        bool notMaxLevel = model.Level < farmSettings.MaxFarmLevel;

        bool showButton = notMaxLevel && (levelEnough || moneyEnough);
        bool enableButton = notMaxLevel && levelEnough && moneyEnough;

        upgradeView.SetButtonVisible(showButton);
        upgradeView.SetButtonInteractable(enableButton);
        upgradeView.SetCost(farmSettings.UpgradeCost);
    }


    private void HandleUpgradeClick()
    {
        if (playerLevelService.CurrentLevel < farmSettings.RequiredPlayerLevelForUpgrade)
        {
            Debug.Log("[Farm] Player level too low to upgrade.");
            return;
        }

        if (!economyService.TrySpendMoney(farmSettings.UpgradeCost))
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
}