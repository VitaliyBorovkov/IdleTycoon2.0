using UnityEngine;
using UnityEngine.UI;

public class FarmController : MonoBehaviour
{
    [SerializeField] private FarmView view;
    [SerializeField] private GameObject farmerBotPrefab;
    [SerializeField] private FarmSettings farmSettings;
    [SerializeField] private Button upgradeButton;

    private PlayerLevelSystem playerLevelSystem;
    private InventoryController inventory;
    private EconomyController economy;

    private FarmModel model;
    private FarmerBotController farmerBot;

    public FarmSettings GetSettings() => farmSettings;
    public int GetUpgradeCost() => farmSettings.UpgradeCost;
    public int GetRequiredPlayerLevelForUpgrade() => farmSettings.RequiredPlayerLevelForUpgrade;

    public void Initialize(Transform storagePoint, PlayerLevelSystem playerLevelSystem, InventoryController inventory,
        EconomyController economy)
    {
        this.playerLevelSystem = playerLevelSystem;
        this.inventory = inventory;
        this.economy = economy;

        if (playerLevelSystem.CurrentLevel < farmSettings.RequiredPlayerLevelForBuild)
        {
            Debug.LogWarning($"Player level too low to build this farm. Required: {farmSettings.RequiredPlayerLevelForBuild}");
            Destroy(gameObject);
            return;
        }


        if (!economy.TrySpendMoney(farmSettings.BuildCost))
        {
            Debug.LogWarning("Not enough money to build farm");
            Destroy(gameObject);
            return;
        }

        model = new FarmModel();
        SpawnFarmerBot(storagePoint, playerLevelSystem);
        InitializeUpgradeButton();
    }

    private void SpawnFarmerBot(Transform storagePoint, PlayerLevelSystem playerLevelSystem)
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

    private void InitializeUpgradeButton()
    {
        if (upgradeButton == null)
        {
            Debug.LogWarning("Upgrade button is not assigned.");
            return;
        }

        upgradeButton.onClick.AddListener(HandleUpgradeClick);
        upgradeButton.interactable = playerLevelSystem.CurrentLevel >= farmSettings.RequiredPlayerLevelForUpgrade;
        playerLevelSystem.OnLevelUp += _ => UpdateUpgradeButtonState();
    }

    private void UpdateUpgradeButtonState()
    {
        //  if (upgradeButton == null)
        //  {
        //      return;
        //  }

        //  bool canUpgrade =
        //model.Level < farmSettings.MaxFarmLevel &&
        //playerLevelSystem.CurrentLevel >= farmSettings.RequiredPlayerLevelForUpgrade;

        upgradeButton.gameObject.SetActive(/*canUpgrade*/true);

        //upgradeButton.interactable = playerLevelSystem.CurrentLevel >= farmSettings.RequiredPlayerLevelForUpgrade;
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

        UpdateUpgradeButtonState();
    }
}