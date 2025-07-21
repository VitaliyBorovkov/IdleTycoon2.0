using System.Collections;

using UnityEngine;

public class BakeryController : MonoBehaviour
{
    [SerializeField] private BakerySettings settings;

    private IInventoryService inventoryService;
    private BakeryModel model;

    public void Initialize(IInventoryService inventoryService)
    {
        this.inventoryService = inventoryService;
        model = new BakeryModel();

        StartCoroutine(ProcessingLoop());
    }

    private IEnumerator ProcessingLoop()
    {
        while (true)
        {
            yield return null;

            if (model.IsProcessing)
                continue;

            if (inventoryService.GetAmount(ItemType.Flour) >= settings.flourPerBatch)
            {
                model.SetProcessing(true);

                inventoryService.TryConsume(ItemType.Flour, settings.flourPerBatch);
                Debug.Log($"[Bakery] Started processing flour...");

                yield return new WaitForSeconds(settings.craftTime);

                inventoryService.Add(ItemType.Bread, settings.breadPerBatch);
                Debug.Log($"[Bakery] Produced {settings.breadPerBatch} bread.");

                model.SetProcessing(false);
            }
        }
    }
}