using UnityEngine;

public class InventoryPanelController : MonoBehaviour
{
    [SerializeField] private InventoryPanelView view;
    [SerializeField] private MonoBehaviour inventoryServiceSource;

    private IInventoryService inventoryService;

    private bool isVisible = false;

    public void Initialize()
    {
        inventoryService = inventoryServiceSource as IInventoryService;
        inventoryService.OnInventoryChanged += HandleInventoryChanged;
        view.SetActive(false);
    }

    private void UpdateInventory()
    {
        view.UpdateInventory(
            inventoryService.GetAmount(ItemType.Grain),
            inventoryService.GetAmount(ItemType.Flour),
            inventoryService.GetAmount(ItemType.Bread)
        );
    }
    private void HandleInventoryChanged()
    {
        if (isVisible)
        {
            UpdateInventory();
        }
    }

    public void OpenInventoryPanel()
    {
        isVisible = !isVisible;
        view.SetActive(isVisible);

        if (isVisible)
        {
            UpdateInventory();
        }
    }

    private void OnDestroy()
    {
        inventoryService.OnInventoryChanged -= HandleInventoryChanged;
    }
}
