using System.Collections;

using UnityEngine;

public class MillController : MonoBehaviour
{
    [SerializeField] private MillSettings settings;
    private InventoryController inventory;
    private MillModel model;

    public void Initialize(InventoryController inventory)
    {
        this.inventory = inventory;
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

            if (inventory.GetAmount(ItemType.Grain) >= settings.grainPerBatch)
            {
                model.SetProcessing(true);

                inventory.TryConsume(ItemType.Grain, settings.grainPerBatch);
                Debug.Log("[Mill] Started processing grain...");

                yield return new WaitForSeconds(settings.craftTime);

                inventory.Add(ItemType.Flour, settings.flourPerBatch);
                Debug.Log($"[Mill] Produced {settings.flourPerBatch} flour.");

                model.SetProcessing(false);
            }
        }
    }
}