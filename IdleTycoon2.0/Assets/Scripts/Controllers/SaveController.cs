using System.Collections.Generic;

public class SaveController
{
    private readonly SaveService saveService;

    private readonly IPlayerLevelService playerLevel;
    private readonly IEconomyService economy;
    private readonly IInventoryService inventory;

    private readonly List<IBuildingSave> buildingSaveProviders;
    private readonly List<IBotSave> botSaveProviders;

    public SaveController(SaveService saveService, IPlayerLevelService playerLevel, IEconomyService economy,
        IInventoryService inventory, List<IBuildingSave> buildingSaveProviders, List<IBotSave> botSaveProviders)
    {
        this.saveService = saveService;
        this.playerLevel = playerLevel;
        this.economy = economy;
        this.inventory = inventory;
        this.buildingSaveProviders = buildingSaveProviders;
        this.botSaveProviders = botSaveProviders;
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData();

        saveData.playerData = new PlayerData
        {
            playerLevel = playerLevel.CurrentLevel,
            xp = playerLevel.CurrentXP,
            money = economy.GetMoney()
        };

        saveData.inventoryData = new InventoryData
        {
            grain = inventory.GetAmount(ItemType.Grain),
            flour = inventory.GetAmount(ItemType.Flour),
            bread = inventory.GetAmount(ItemType.Bread)
        };

        List<BuildingData> buildings = new List<BuildingData>();
        foreach (var provider in buildingSaveProviders)
        {
            buildings.Add(provider.GetBuildingData());
        }
        saveData.buildingsData = buildings.ToArray();

        List<BotData> bots = new List<BotData>();
        foreach (var provider in botSaveProviders)
        {
            bots.Add(provider.GetBotData());
        }
        saveData.botsData = bots.ToArray();

        saveService.SaveGame(saveData);
    }
}