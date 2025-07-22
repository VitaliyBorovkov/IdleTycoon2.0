using System.Collections.Generic;

using UnityEngine;

public class LoadController
{
    private readonly SaveService saveService;
    private readonly IEconomyService economy;
    private readonly IInventoryService inventory;
    private readonly IPlayerLevelService playerLevel;

    private readonly GameManager gameManager;
    private readonly BuildSlotController[] buildSlots;

    private readonly GameObject farmPrefab;
    private readonly GameObject farmerBotPrefab;
    private readonly GameObject millerBotPrefab;
    private readonly GameObject bakerBotPrefab;

    private readonly Transform millPoint;
    private readonly Transform bakeryPoint;
    private readonly Transform warehousePoint;
    private readonly Transform FarmerSpawnPoint;

    private readonly BakeryController bakeryController;

    private bool isGameLoaded = false;

    private Dictionary<int, FarmController> loadedFarms = new Dictionary<int, FarmController>();

    public LoadController(SaveService saveService, IPlayerLevelService playerLevel, IEconomyService economy,
        IInventoryService inventory, GameManager gameManager, BuildSlotController[] buildSlots,
        GameObject farmPrefab, GameObject farmerBotPrefab, GameObject millerBotPrefab,
        GameObject bakerBotPrefab, Transform millPoint, Transform bakeryPoint, Transform warehousePoint,
        Transform farmerSpawnPoint, BakeryController bakeryController)

    {
        this.economy = economy;
        this.inventory = inventory;
        this.buildSlots = buildSlots;
        this.saveService = saveService;
        this.playerLevel = playerLevel;
        this.gameManager = gameManager;

        this.farmPrefab = farmPrefab;
        this.bakerBotPrefab = bakerBotPrefab;
        this.farmerBotPrefab = farmerBotPrefab;
        this.millerBotPrefab = millerBotPrefab;

        this.millPoint = millPoint;
        this.bakeryPoint = bakeryPoint;
        this.warehousePoint = warehousePoint;
        this.bakeryController = bakeryController;

        FarmerSpawnPoint = farmerSpawnPoint;
    }

    public void LoadGame()
    {
        if (isGameLoaded)
        {
            Debug.LogWarning("[LoadController] LoadGame() called more than once. Ignoring.");
            return;
        }

        isGameLoaded = true;

        SaveData data = saveService.LoadGame();
        if (data == null)
        {
            Debug.LogWarning("[LoadController] No saved data to load.");

            GameObject millerGO = GameObject.Instantiate(millerBotPrefab, millPoint.position, Quaternion.identity);
            var miller = millerGO.GetComponent<MillerBotController>();
            miller.Initialize(inventory, bakeryPoint, playerLevel, economy, bakeryController);
            gameManager.RegisterBot(miller);

            GameObject bakerGO = GameObject.Instantiate(bakerBotPrefab, bakeryPoint.position, Quaternion.identity);
            var baker = bakerGO.GetComponent<BakerBotController>();
            baker.Initialize(inventory, warehousePoint, playerLevel, economy);
            gameManager.RegisterBot(baker);

            return;
        }


        playerLevel.SetLevel(data.playerData.playerLevel);
        playerLevel.SetXP(data.playerData.xp);
        economy.SetMoney(data.playerData.money);

        inventory.SetItem(ItemType.Grain, data.inventoryData.grain);
        inventory.SetItem(ItemType.Flour, data.inventoryData.flour);
        inventory.SetItem(ItemType.Bread, data.inventoryData.bread);

        foreach (var mono in GameObject.FindObjectsOfType<MonoBehaviour>())
        {
            if (mono is IBuildingSave && mono.name.Contains("(Clone)"))
            {
                GameObject.Destroy(mono.gameObject);
            }
        }

        foreach (var building in data.buildingsData)
        {
            if (building.slotIndex < 0 || building.slotIndex >= buildSlots.Length)
            {
                Debug.LogWarning($"[LoadController] Invalid slot index: {building.slotIndex}");
                continue;
            }

            var slot = buildSlots[building.slotIndex];
            slot.BuildFromSave(farmPrefab, building.level);

            FarmController farmController = slot.GetFarm();
            if (farmController != null)
            {
                loadedFarms[building.slotIndex] = farmController;
            }
        }

        foreach (var bot in data.botsData)
        {
            switch (bot.botType)
            {
                case "FarmerBot":
                    {
                        var farm = buildSlots[bot.slotIndex].GetFarm();
                        if (farm == null)
                        {
                            Debug.LogError("[LoadController] No Farm found to assign FarmerBot.");
                            continue;
                        }

                        var spawnPoint = farm.GetComponent<FarmView>().GetBotSpawnPoint().position;
                        GameObject botGO = GameObject.Instantiate(farmerBotPrefab, spawnPoint, Quaternion.identity);

                        var farmer = botGO.GetComponent<FarmerBotController>();
                        farmer.Initialize(farm.MillPoint, farm.FarmModel, playerLevel, inventory, economy,
                            bot.botLevel, spawnPoint, bot.slotIndex);

                        if (loadedFarms.TryGetValue(bot.slotIndex, out var farmController))
                        {
                            farmController.SetFarmerBot(farmer);
                        }
                        else
                        {
                            Debug.LogWarning($"[LoadController] Cannot find FarmController for slot {bot.slotIndex}, bot not assigned.");
                        }

                        gameManager.RegisterBot(farmer);
                        break;
                    }
                case "MillerBot":
                    {
                        if (GameObject.FindObjectOfType<MillerBotController>() != null)
                        {
                            break;
                        }

                        GameObject botGO = GameObject.Instantiate(millerBotPrefab, millPoint.position, Quaternion.identity);
                        var miller = botGO.GetComponent<MillerBotController>();
                        miller.Initialize(inventory, bakeryPoint, playerLevel, economy, bakeryController);
                        gameManager.RegisterBot(miller);
                        break;
                    }
                case "BakerBot":
                    {
                        if (GameObject.FindObjectOfType<BakerBotController>() != null)
                        {
                            break;
                        }

                        GameObject botGO = GameObject.Instantiate(bakerBotPrefab, bakeryPoint.position, Quaternion.identity);
                        var baker = botGO.GetComponent<BakerBotController>();
                        baker.Initialize(inventory, warehousePoint, playerLevel, economy);
                        gameManager.RegisterBot(baker);
                        break;
                    }
                default:
                    Debug.LogWarning($"[LoadController] Unknown bot type: {bot.botType}");
                    continue;
            }
        }
        Debug.Log("[LoadController] Game loaded successfully.");
    }
}