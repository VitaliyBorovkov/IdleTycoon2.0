using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EconomyController economyController;
    [SerializeField] private FarmController farmController;
    [SerializeField] private Transform storagePoint;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        Debug.Log("[GameManager] Initializing...");

        economyController.Initialize();
        farmController.Initialize(storagePoint);
    }
}
