using System.Collections;

using UnityEngine;

public class FarmerBotController : BotBase, IBotSave
{
    [SerializeField] private FarmerStatsDatabase statsDatabase;

    private Transform millPoint;
    private FarmModel farmModel;
    private FarmerStats settings;

    private IEconomyService economyService;
    private IInventoryService inventoryService;
    private IPlayerLevelService playerLevelService;

    private Vector3 startPoint;

    private int botLevel;
    private int slotIndex;

    private bool isInitialized = false;

    public void Initialize(Transform millPoint, FarmModel farmModel, IPlayerLevelService playerLevelService,
        IInventoryService inventoryService, IEconomyService economyService, int botLevel, Vector3 spawnPoint,
        int slotIndex)
    {
        this.millPoint = millPoint;
        this.playerLevelService = playerLevelService;
        this.inventoryService = inventoryService;
        this.economyService = economyService;
        this.farmModel = farmModel;
        this.botLevel = botLevel;
        this.startPoint = spawnPoint;
        this.slotIndex = slotIndex;

        isInitialized = true;

        int level = farmModel.Level;

        settings = GetStatsForLevel(level);
        if (settings == null)
        {
            Debug.LogError($"[FarmerBot] Failed to initialize stats for level {level}. Destroying bot.");
            Destroy(gameObject);
            return;
        }

        StartCoroutine(WaitForInitThenWorkLoop());
    }

    private FarmerStats GetStatsForLevel(int level)
    {
        if (statsDatabase.levels == null || level < 0 || level >= statsDatabase.levels.Length)
        {
            Debug.LogError($"[FarmerBot] Stats not found for level {level}");
            return null;
        }

        if (statsDatabase.levels[level] == null)
        {
            Debug.LogError($"[FarmerBot] Level {level} in FarmerStatsDatabase is NULL!");
            return null;
        }

        return statsDatabase.levels[level];
    }

    public void UpgradeStats(int level)
    {
        settings = GetStatsForLevel(level);
        if (settings == null)
        {
            Debug.LogError($"[FarmerBot] Stats not found for level {level}. Check FarmerStatsDatabase!");
            Destroy(gameObject);
            return;
        }
    }

    private IEnumerator WaitForInitThenWorkLoop()
    {
        while (!isInitialized || settings == null)
        {
            yield return null;
        }

        StartCoroutine(WorkLoop());
    }

    private IEnumerator WorkLoop()
    {
        while (true)
        {

            yield return new WaitForSeconds(settings.harvestTime);

            Debug.Log($"[FarmerBot] Produced {settings.grainPerHarvest} grain(s). Moving to mill...");

            yield return StartCoroutine(MoveTo(millPoint.position, settings.moveSpeed));

            yield return new WaitForSeconds(0.5f);
            Debug.Log($"[FarmerBot] Delivered to mill.");
            playerLevelService.AddXP(settings.xpPerCycle);
            economyService.AddMoney(settings.moneyPerCycle);
            inventoryService.Add(ItemType.Grain, settings.grainPerHarvest);
            yield return StartCoroutine(MoveTo(startPoint, settings.moveSpeed));
        }
    }

    public BotData GetBotData()
    {
        return new BotData
        {
            botType = "FarmerBot",
            botLevel = botLevel,
            slotIndex = this.slotIndex
        };
    }
}