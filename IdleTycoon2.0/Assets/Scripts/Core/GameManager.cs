using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EconomyController economyController;
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private EconomyUIController economyUIController;
    [SerializeField] private MillController millController;
    [SerializeField] private PlayerLevelSystem playerLevelSystem;
    [SerializeField] private XPBarController xpBarController;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        economyController.Initialize();
        inventoryController.Initialize();
        economyUIController.Initialize();
        xpBarController.Initialize(playerLevelSystem);
        millController.Initialize(inventoryController);
    }
}
