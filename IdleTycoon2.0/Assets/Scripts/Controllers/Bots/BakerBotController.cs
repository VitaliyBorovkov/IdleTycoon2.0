using System.Collections;

using UnityEngine;

public class BakerBotController : MonoBehaviour
{
    [SerializeField] private BakerSettings settings;

    private IInventoryService inventoryService;
    private Transform warehousePoint;

    private Vector3 startPoint;

    public void Initialize(IInventoryService inventoryService, Transform warehousePoint)
    {
        this.inventoryService = inventoryService;
        this.warehousePoint = warehousePoint;
        startPoint = transform.position;

        StartCoroutine(WorkLoop());
    }

    private IEnumerator WorkLoop()
    {
        while (true)
        {
            while (inventoryService.GetAmount(ItemType.Flour) < settings.flourPerBatch)
            {
                yield return new WaitForSeconds(0.5f);
            }

            bool success = inventoryService.TryConsume(ItemType.Flour, settings.flourPerBatch);
            if (!success)
            {
                Debug.LogWarning("[BakerBot] Failed to consume flour after check.");
                continue;
            }

            Debug.Log($"[BakerBot] Baking bread from {settings.flourPerBatch} flour...");
            yield return new WaitForSeconds(settings.craftTime);

            inventoryService.Add(ItemType.Bread, settings.breadPerBatch);
            Debug.Log($"[BakerBot] Produced {settings.breadPerBatch} bread. Delivering to warehouse...");

            yield return StartCoroutine(MoveTo(warehousePoint.position));

            yield return new WaitForSeconds(0.5f);
            Debug.Log("[BakerBot] Delivered bread to warehouse.");

            yield return StartCoroutine(MoveTo(startPoint));
        }
    }

    private IEnumerator MoveTo(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, settings.moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}