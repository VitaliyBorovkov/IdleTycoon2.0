using System;

[Serializable]
public class SaveData
{
    public PlayerData playerData;
    public InventoryData inventoryData;
    public BuildingData[] buildingsData;
    public BotData[] botsData;
}

[Serializable]
public class PlayerData
{
    public int playerLevel;
    public int xp;
    public int money;
}

[Serializable]
public class InventoryData
{
    public int grain;
    public int flour;
    public int bread;
}

[Serializable]
public class BuildingData
{
    public int slotIndex;
    public string type;
    public int level;
}

[Serializable]
public class BotData
{
    public string botType;
    public int botLevel;
    public int slotIndex;
}