using System.Collections;

using UnityEngine;

public class MillController : MonoBehaviour
{
    [SerializeField] private MillSettings settings;

    private IInventoryService inventoryService;
    private MillModel model;

    public void Initialize(IInventoryService inventoryService)
    {
        this.inventoryService = inventoryService;
        model = new MillModel();

        StartCoroutine(ProcessingLoop());
    }

    private IEnumerator ProcessingLoop()
    {
        while (true)
        {
            yield return null;

            if (model.IsProcessing)
                continue;

            if (inventoryService.GetAmount(ItemType.Grain) >= settings.grainPerBatch)
            {
                model.SetProcessing(true);

                inventoryService.TryConsume(ItemType.Grain, settings.grainPerBatch);
                Debug.Log("[Mill] Started processing grain...");

                yield return new WaitForSeconds(settings.craftTime);

                inventoryService.Add(ItemType.Flour, settings.flourPerBatch);
                Debug.Log($"[Mill] Produced {settings.flourPerBatch} flour.");

                model.SetProcessing(false);
            }
        }
    }
}