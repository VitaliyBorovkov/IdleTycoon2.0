using System.Collections;

using UnityEngine;

public class FarmerBotController : MonoBehaviour
{
    private Transform storagePoint;
    private FarmModel model;

    private Vector3 startPoint;
    private float speed = 2f;

    public void Initialize(Transform storagePoint, FarmModel farmModel)
    {
        this.storagePoint = storagePoint;
        this.model = farmModel;

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