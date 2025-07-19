using UnityEngine;

public class FarmController : MonoBehaviour
{
    [SerializeField] private FarmView view;
    [SerializeField] private GameObject farmerBotPrefab;
    [SerializeField] private FarmLevelSettings levelSettings;

    private FarmModel model;
    private FarmerBotController farmerBot;

    public void Initialize(Transform storagePoint)
    {
        model = new FarmModel(levelSettings.level, levelSettings.productionInterval, levelSettings.grainPerCycle);
        SpawnFarmerBot(storagePoint);
    }

    private void SpawnFarmerBot(Transform storagePoint)
    {
        if (farmerBotPrefab == null)
        {
            Debug.LogError("FarmerBot prefab is not assigned!");
            return;
        }

        GameObject botGO = Instantiate(farmerBotPrefab, view.GetBotSpawnPoint().position, Quaternion.identity);
        farmerBot = botGO.GetComponent<FarmerBotController>();
        farmerBot.Initialize(storagePoint, model);
    }
}