using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EconomyController economyController;
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private EconomyUIController economyUIController;
    [SerializeField] private Transform storagePoint;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        Debug.Log("[GameManager] Initializing...");

        economyController.Initialize();
        inventoryController.Initialize();
        economyUIController.Initialize();
    }
}
