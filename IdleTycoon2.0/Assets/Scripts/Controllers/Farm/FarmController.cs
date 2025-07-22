using UnityEngine;


public class FarmController : MonoBehaviour, IBuildingSave
{
    [SerializeField] private FarmView view;
    [SerializeField] private GameObject farmerBotPrefab;
    [SerializeField] private FarmUpgradeView upgradeView;

    private Transform millPoint;
    private FarmModel farmModel;

    public FarmModel FarmModel => farmModel;
    public Transform MillPoint => millPoint;

    private FarmSettings farmSettings;
    private IEconomyService economyService;
    private IInventoryService inventoryService;
    private IPlayerLevelService playerLevelService;

    private FarmerBotController farmerBot;

    private int slotIndex;

    private bool skipBotSpawn = false;

    public FarmSettings GetSettings() => farmSettings;

    public void Initialize(Transform millPoint, IPlayerLevelService playerLevelService, IInventoryService inventoryService,
        IEconomyService economyService, FarmSettings farmSettings, int slotIndex)
    {
        this.slotIndex = slotIndex;
        this.millPoint = millPoint;
        this.farmSettings = farmSettings;
        this.economyService = economyService;
        this.inventoryService = inventoryService;
        this.playerLevelService = playerLevelService;

        if (playerLevelService.CurrentLevel < farmSettings.RequiredPlayerLevelForBuild)
        {
            Debug.LogWarning($"Player level too low to build this farm. Required: {farmSettings.RequiredPlayerLevelForBuild}");
            return;
        }

        farmModel = new FarmModel();
        if (!skipBotSpawn)
        {
            SpawnFarmerBot(millPoint);
        }

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

        Vector3 spawnPoint = view.GetBotSpawnPoint().position;
        int botLevel = farmModel.Level;

        GameObject botGO = Instantiate(farmerBotPrefab, spawnPoint, Quaternion.identity);
        farmerBot = botGO.GetComponent<FarmerBotController>();
        farmerBot.Initialize(millPoint, farmModel, playerLevelService, inventoryService, economyService,
        botLevel, spawnPoint, slotIndex);

        FindObjectOfType<GameManager>().RegisterBot(farmerBot);
    }

    private void UpdateUpgradeUI()
    {
        if (farmModel.Level >= farmSettings.MaxFarmLevel)
        {
            upgradeView.SetButtonVisible(false);
            return;
        }

        var next = GetNextLevelSettings();

        bool levelEnough = playerLevelService.CurrentLevel >= next.requiredPlayerLevel;
        bool moneyEnough = economyService.GetMoney() >= next.upgradeCost;

        bool showButton = levelEnough;
        bool enableButton = levelEnough && moneyEnough;

        upgradeView.SetButtonVisible(showButton);
        upgradeView.SetButtonInteractable(enableButton);
        upgradeView.SetCost(next.upgradeCost);
    }

    private void HandleUpgradeClick()
    {
        if (farmModel.Level >= farmSettings.MaxFarmLevel)
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

        if (farmModel.Level >= farmSettings.MaxFarmLevel)
        {
            Debug.Log("[Farm] Farm is already at max level.");
            return;
        }

        farmModel.Upgrade();

        if (farmerBot != null)
        {
            farmerBot.UpgradeStats(farmModel.Level);
        }
        else
        {
            Debug.LogWarning("[FarmController] Farmer bot is null. Skipping bot upgrade.");
        }

        Debug.Log($"[Farm] Farm upgraded to level {farmModel.Level}");

        UpdateUpgradeUI();
    }

    private FarmLevelSettings GetNextLevelSettings()
    {
        return farmSettings.GetNextLevelSettings(farmModel.Level);
    }

    public BuildingData GetBuildingData()
    {
        return new BuildingData
        {
            slotIndex = slotIndex,
            type = "Farm",
            level = farmModel.Level
        };
    }

    public void UpgradeToLevel(int newLevel)
    {
        farmModel.SetLevel(newLevel);

        //if (farmerBot == null)
        //{
        //    Debug.LogWarning("[FarmController] Farmer bot not found after load. Respawning...");
        //    SpawnFarmerBot(millPoint);
        //}

        if (farmerBot != null)
        {
            farmerBot.UpgradeStats(newLevel);
        }
        else
        {
            Debug.LogWarning("[FarmController] Farmer bot is null. Skipping bot upgrade.");
        }

        Debug.Log($"[FarmController] Force set level to {newLevel}");
    }

    public void SetSkipBotSpawn(bool value)
    {
        skipBotSpawn = value;
    }

    public void SetFarmerBot(FarmerBotController bot)
    {
        farmerBot = bot;
    }
}