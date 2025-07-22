using UnityEngine;

public class BuildSlotController : MonoBehaviour
{
    [SerializeField] private int requiredLevel = 1;
    [SerializeField] private int farmIndex;

    [SerializeField] private MonoBehaviour playerLevelServiceSource;
    [SerializeField] private MonoBehaviour inventoryServiceSource;
    [SerializeField] private MonoBehaviour economyServiceSource;

    [SerializeField] private FarmSettings farmSettings;
    [SerializeField] private GameObject farmPrefab;
    [SerializeField] private Transform farmSpawnPoint;
    [SerializeField] private Transform millPoint;

    private IEconomyService economyService;
    private IPlayerLevelService playerLevelService;
    private IInventoryService inventoryService;
    private FarmController currentFarm;
    private bool isUnlocked = false;

    public Transform FarmerSpawnPoint;

    public bool IsOccupied { get; private set; }
    public bool IsUnlocked => isUnlocked;

    private void Start()
    {
        economyService = economyServiceSource as IEconomyService;
        inventoryService = inventoryServiceSource as IInventoryService;
        playerLevelService = playerLevelServiceSource as IPlayerLevelService;

        if (playerLevelService.CurrentLevel >= requiredLevel)
        {
            Unlock();
        }
        else
        {
            playerLevelService.OnLevelUp += CheckUnlock;
        }
    }

    private void CheckUnlock(int newLevel)
    {
        if (newLevel >= requiredLevel && !isUnlocked)
        {
            Unlock();
        }
    }

    private void Unlock()
    {
        isUnlocked = true;
        playerLevelService.OnLevelUp -= CheckUnlock;
    }

    private void OnMouseDown()
    {
        if (!isUnlocked || IsOccupied)
        {
            return;
        }

        if (farmSettings == null)
        {
            Debug.LogError($"[BuildSlot] Invalid farmIndex: {farmIndex}");
            return;
        }

        if (playerLevelService.CurrentLevel < farmSettings.RequiredPlayerLevelForBuild)
        {
            Debug.LogWarning($"[BuildSlot] Not enough level to build farm. Required: {farmSettings.RequiredPlayerLevelForBuild}");
            return;
        }

        if (!economyService.TrySpendMoney(farmSettings.BuildCost))
        {
            Debug.LogWarning("[BuildSlot] Not enough money to build farm.");
            return;
        }

        GameObject farmGO = Instantiate(farmPrefab, farmSpawnPoint.position, Quaternion.identity);
        currentFarm = farmGO.GetComponent<FarmController>();
        currentFarm.Initialize(millPoint, playerLevelService, inventoryService, economyService, farmSettings,
            farmIndex);

        FindObjectOfType<GameManager>().RegisterBuilding(currentFarm);

        IsOccupied = true;
    }

    public FarmController GetFarm()
    {
        return currentFarm;
    }

    public void BuildFromSave(GameObject farmPrefab, int level)
    {
        if (IsOccupied)
            return;

        GameObject farmGO = GameObject.Instantiate(farmPrefab, farmSpawnPoint.position, Quaternion.identity);
        var farm = farmGO.GetComponent<FarmController>();
        farm.SetSkipBotSpawn(true);
        farm.Initialize(millPoint, playerLevelService, inventoryService, economyService, farmSettings, farmIndex);
        farm.UpgradeToLevel(level);

        currentFarm = farm;
        IsOccupied = true;

        FindObjectOfType<GameManager>().RegisterBuilding(farm);
    }
}
