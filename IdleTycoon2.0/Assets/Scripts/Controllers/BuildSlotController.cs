using UnityEngine;

public class BuildSlotController : MonoBehaviour
{
    [SerializeField] private int requiredLevel = 1;
    [SerializeField] private int farmIndex = 0;

    [SerializeField] private MonoBehaviour playerLevelServiceSource;
    [SerializeField] private MonoBehaviour inventoryServiceSource;
    [SerializeField] private MonoBehaviour economyServiceSource;

    [SerializeField] private FarmSettingsDatabase settingsDatabase;
    [SerializeField] private GameObject farmPrefab;
    [SerializeField] private Transform farmSpawnPoint;
    [SerializeField] private Transform millPoint;

    private IEconomyService economyService;
    private IPlayerLevelService playerLevelService;
    private IInventoryService inventoryService;
    private FarmController currentFarm;
    private bool isUnlocked = false;

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
        Debug.Log($"[BuildSlot] Unlocked at level {requiredLevel}");
    }

    private void OnMouseDown()
    {
        if (!isUnlocked || IsOccupied)
        {
            return;
        }

        if (farmIndex >= settingsDatabase.levels.Length)
        {
            Debug.LogError($"[BuildSlot] Invalid farmIndex: {farmIndex}");
            return;
        }

        var farmSettings = settingsDatabase.levels[farmIndex];

        GameObject farmGO = Instantiate(farmPrefab, farmSpawnPoint.position, Quaternion.identity);
        currentFarm = farmGO.GetComponent<FarmController>();
        currentFarm.Initialize(millPoint, playerLevelService, inventoryService, economyService, farmSettings);

        IsOccupied = true;
        Debug.Log($"[BuildSlot] Farm built at slot (requiredLevel = {requiredLevel})");
    }

    public FarmController GetFarm()
    {
        return currentFarm;
    }
}
