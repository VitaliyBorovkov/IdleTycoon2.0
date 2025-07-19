using UnityEngine;

public class BuildSlotController : MonoBehaviour
{
    [SerializeField] private int requiredLevel = 1;
    [SerializeField] private int farmIndex = 0;

    [SerializeField] private PlayerLevelSystem playerLevelSystem;
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private EconomyController economyController;

    [SerializeField] private FarmSettingsDatabase settingsDatabase;
    [SerializeField] private GameObject farmPrefab;
    [SerializeField] private Transform farmSpawnPoint;
    [SerializeField] private Transform storagePoint;

    private FarmController currentFarm;
    private bool isUnlocked = false;

    public bool IsOccupied { get; private set; }
    public bool IsUnlocked => isUnlocked;

    private void Start()
    {
        if (playerLevelSystem.CurrentLevel >= requiredLevel)
        {
            Unlock();
        }
        else
        {
            playerLevelSystem.OnLevelUp += CheckUnlock;
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
        playerLevelSystem.OnLevelUp -= CheckUnlock;
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
        currentFarm.Initialize(storagePoint, playerLevelSystem, inventoryController, economyController, farmSettings);

        IsOccupied = true;
        Debug.Log($"[BuildSlot] Farm built at slot (requiredLevel = {requiredLevel})");
    }

    public FarmController GetFarm()
    {
        return currentFarm;
    }
}
