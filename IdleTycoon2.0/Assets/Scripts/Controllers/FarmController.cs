using UnityEngine;

public class FarmController : MonoBehaviour
{
    [SerializeField] private FarmView view;
    [SerializeField] private GameObject farmerBotPrefab;
    [SerializeField] private FarmLevelSettings levelSettings;
    /*[SerializeField]*/
    private PlayerLevelSystem playerLevelSystem;
    /*[SerializeField]*/
    private InventoryController inventory;

    private FarmModel model;
    private FarmerBotController farmerBot;

    public void Initialize(Transform storagePoint, PlayerLevelSystem playerLevelSystem, InventoryController inventory)
    {
        this.playerLevelSystem = playerLevelSystem;
        this.inventory = inventory;

        model = new FarmModel(levelSettings.level, levelSettings.productionInterval, levelSettings.grainPerCycle);
        SpawnFarmerBot(storagePoint, playerLevelSystem/*, inventory*/);
    }

    private void SpawnFarmerBot(Transform storagePoint, PlayerLevelSystem playerLevelSystem/*, InventoryController inventory*/)
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
}