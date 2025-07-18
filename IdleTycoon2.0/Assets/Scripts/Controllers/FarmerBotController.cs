using System.Collections;

using UnityEngine;

public class FarmerBotController : MonoBehaviour
{
    private Transform storagePoint;
    private FarmModel model;
    private PlayerLevelSystem playerLevelSystem;
    private InventoryController inventory;

    private Vector3 startPoint;
    private float speed = 2f;

    public void Initialize(Transform storagePoint, FarmModel farmModel, PlayerLevelSystem playerLevelSystem, InventoryController inventory)
    {
        this.storagePoint = storagePoint;
        this.model = farmModel;
        this.playerLevelSystem = playerLevelSystem;
        this.inventory = inventory;

        startPoint = transform.position;

        StartCoroutine(WorkLoop());
    }

    private IEnumerator WorkLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(model.ProductionInterval);

            Debug.Log($"[FarmerBot] Produced {model.GrainPerCycle} grain(s). Moving to storage...");

            yield return StartCoroutine(MoveTo(storagePoint.position));

            yield return new WaitForSeconds(0.5f);
            Debug.Log($"[FarmerBot] Delivered to storage.");
            playerLevelSystem.AddXP(5);
            inventory.Add(ItemType.Grain, model.GrainPerCycle);
            yield return StartCoroutine(MoveTo(startPoint));
        }


    }

    private IEnumerator MoveTo(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
    }
}