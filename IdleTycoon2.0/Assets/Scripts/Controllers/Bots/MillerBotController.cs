using System.Collections;

using UnityEngine;

public class MillerBotController : MonoBehaviour
{
    [SerializeField] private MillerSettings settings;

    private IInventoryService inventoryService;
    private Transform bakeryPoint;

    private Vector3 startPoint;

    public void Initialize(IInventoryService inventoryService, Transform bakeryPoint)
    {
        this.inventoryService = inventoryService;
        this.bakeryPoint = bakeryPoint;
        startPoint = transform.position;

        StartCoroutine(WorkLoop());
    }

    private IEnumerator WorkLoop()
    {
        while (true)
        {
            while (inventoryService.GetAmount(ItemType.Grain) < settings.grainPerBatch)
            {
                yield return new WaitForSeconds(0.5f);
            }

            bool success = inventoryService.TryConsume(ItemType.Grain, settings.grainPerBatch);
            if (!success)
            {
                Debug.LogWarning("[MillerBot] Failed to consume grain after check.");
                continue;
            }

            Debug.Log($"[MillerBot] Crafting flour from {settings.grainPerBatch} grain...");
            yield return new WaitForSeconds(settings.craftTime);

            inventoryService.Add(ItemType.Flour, settings.flourPerBatch);
            Debug.Log($"[MillerBot] Produced {settings.flourPerBatch} flour. Delivering to bakery...");

            yield return StartCoroutine(MoveTo(bakeryPoint.position));

            yield return new WaitForSeconds(0.5f);

            Debug.Log($"[MillerBot] Delivered flour to bakery. Returning...");
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