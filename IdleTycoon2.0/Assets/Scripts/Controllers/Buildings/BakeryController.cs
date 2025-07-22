using System.Collections;

using UnityEngine;

public class BakeryController : MonoBehaviour
{
    [SerializeField] private BakerySettings settings;

    private IInventoryService inventoryService;
    private BakeryModel model;

    private int flourAvailableForCraft = 0;

    public void Initialize(IInventoryService inventoryService)
    {
        this.inventoryService = inventoryService;
        model = new BakeryModel();

        StartCoroutine(ProcessingLoop());
    }

    public void NotifyFlourDelivered(int amount)
    {
        flourAvailableForCraft += amount;
        Debug.Log($"[Bakery] Received delivery: {amount} flour. Total available: {flourAvailableForCraft}");
    }

    private IEnumerator ProcessingLoop()
    {
        while (true)
        {
            yield return null;

            if (model.IsProcessing)
                continue;

            if (flourAvailableForCraft >= settings.flourPerBatch)
            {
                model.SetProcessing(true);
                flourAvailableForCraft -= settings.flourPerBatch;

                bool success = inventoryService.TryConsume(ItemType.Flour, settings.flourPerBatch);
                if (!success)
                {
                    Debug.LogWarning("[Bakery] Flour marked as delivered but not found in inventory.");
                    flourAvailableForCraft += settings.flourPerBatch;
                    model.SetProcessing(false);
                    yield return new WaitForSeconds(0.5f);
                    continue;
                }

                Debug.Log($"[Bakery] Started baking bread from {settings.flourPerBatch} flour...");

                yield return new WaitForSeconds(settings.craftTime);

                inventoryService.Add(ItemType.Bread, settings.breadPerBatch);
                Debug.Log($"[Bakery] Produced {settings.breadPerBatch} bread.");

                model.SetProcessing(false);
            }
        }
    }
}