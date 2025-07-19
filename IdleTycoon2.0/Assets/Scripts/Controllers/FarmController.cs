using UnityEngine;


public class FarmController : MonoBehaviour
{
    [SerializeField] private FarmView view;
    [SerializeField] private GameObject farmerBotPrefab;
    [SerializeField] private FarmUpgradeView upgradeView;

    private FarmSettings farmSettings;
    private PlayerLevelSystem playerLevelSystem;
    private InventoryController inventory;
    private EconomyController economy;

    private FarmModel model;
    private FarmerBotController farmerBot;

    public FarmSettings GetSettings() => farmSettings;
    public int GetUpgradeCost() => farmSettings.UpgradeCost;
    public int GetRequiredPlayerLevelForUpgrade() => farmSettings.RequiredPlayerLevelForUpgrade;

    public void Initialize(Transform storagePoint, PlayerLevelSystem playerLevelSystem, InventoryController inventory,
        EconomyController economy, FarmSettings farmSettings)
    {
        this.playerLevelSystem = playerLevelSystem;
        this.inventory = inventory;
        this.economy = economy;
        this.farmSettings = farmSettings;


        if (playerLevelSystem.CurrentLevel < farmSettings.RequiredPlayerLevelForBuild)
        {
            Debug.LogWarning($"Player level too low to build this farm. Required: {farmSettings.RequiredPlayerLevelForBuild}");
            return;
        }


        if (!economy.TrySpendMoney(farmSettings.BuildCost))
        {
            Debug.LogWarning("Not enough money to build farm");
            return;
        }

        model = new FarmModel();
        SpawnFarmerBot(storagePoint);

        upgradeView.Initialize();
        upgradeView.OnUpgradeClicked += HandleUpgradeClick;
        UpdateUpgradeUI();

        playerLevelSystem.OnLevelUp += _ => UpdateUpgradeUI();
        economy.OnMoneyChanged += _ => UpdateUpgradeUI();
    }

    private void SpawnFarmerBot(Transform storagePoint)
    {
        if (farmerBotPrefab == null)
        {
            Debug.LogError("FarmerBot prefab is not assigned!");
            return;
        }

        GameObject botGO = Instantiate(farmerBotPrefab, view.GetBotSpawnPoint().position, Quaternion.identity);
        farmerBot = botGO.GetComponent<FarmerBotController>();
        farmerBot.Initialize(storagePoint, model, playerLevelSystem, inventory);
    }

    private void UpdateUpgradeUI()
    {
        bool canUpgrade =
            model.Level < farmSettings.MaxFarmLevel &&
            playerLevelSystem.CurrentLevel >= farmSettings.RequiredPlayerLevelForUpgrade;

        upgradeView.SetButtonVisible(canUpgrade);
        upgradeView.SetButtonInteractable(canUpgrade);
        upgradeView.SetCost(farmSettings.UpgradeCost);

    }

    private void HandleUpgradeClick()
    {
        if (playerLevelSystem.CurrentLevel < farmSettings.RequiredPlayerLevelForUpgrade)
        {
            Debug.Log("[Farm] Player level too low to upgrade.");
            return;
        }

        if (!economy.TrySpendMoney(farmSettings.UpgradeCost))
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