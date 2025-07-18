using System.Collections;

using UnityEngine;

public class FarmerBotController : MonoBehaviour
{
    [SerializeField] private FarmerStatsDatabase statsDatabase;

    private Transform storagePoint;
    private PlayerLevelSystem playerLevelSystem;
    private InventoryController inventory;
    private FarmerStats currentStats;
    private FarmModel farmModel;

    private Vector3 startPoint;

    public void Initialize(Transform storagePoint, FarmModel farmModel, PlayerLevelSystem playerLevelSystem,
        InventoryController inventory)
    {
        this.storagePoint = storagePoint;
        this.playerLevelSystem = playerLevelSystem;
        this.inventory = inventory;
        this.farmModel = farmModel;

        startPoint = transform.position;

        int level = farmModel.Level;

        currentStats = GetStatsForLevel(level);
        if (currentStats == null)
        {
            Debug.LogError($"[FarmerBot] Failed to initialize stats for level {level}. Destroying bot.");
            Destroy(gameObject);
            return;
        }

        StartCoroutine(WorkLoop());
    }

    private FarmerStats GetStatsForLevel(int level)
    {
        if (statsDatabase.levels == null || statsDatabase.levels.Length < level)
        {
            Debug.LogError($"[FarmerBot] Stats not found for level {level}");
            return null;
        }

        return statsDatabase.levels[level - 1];
    }

    public void UpgradeStats(int newLevel) //upgrade farmer to farm for future
    {
        currentStats = GetStatsForLevel(newLevel);
        if (currentStats == null)
        {
            Debug.LogError($"[FarmerBot] No stats found for level {newLevel}");
        }
    }

    private IEnumerator WorkLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(currentStats.harvestTime);

            Debug.Log($"[FarmerBot] Produced {currentStats.grainPerHarvest} grain(s). Moving to storage...");

            yield return StartCoroutine(MoveTo(storagePoint.position));

            yield return new WaitForSeconds(0.5f);
            Debug.Log($"[FarmerBot] Delivered to storage.");
            playerLevelSystem.AddXP(5);
            inventory.Add(ItemType.Grain, currentStats.grainPerHarvest);
            yield return StartCoroutine(MoveTo(startPoint));
        }
    }

    private IEnumerator MoveTo(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, currentStats.moveSpeed *
                Time.deltaTime);
            yield return null;
        }
    }
}