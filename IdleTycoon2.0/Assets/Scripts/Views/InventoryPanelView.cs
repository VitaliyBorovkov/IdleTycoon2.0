using TMPro;

using UnityEngine;

public class InventoryPanelView : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private TextMeshProUGUI grainText;
    [SerializeField] private TextMeshProUGUI flourText;
    [SerializeField] private TextMeshProUGUI breadText;

    public void SetActive(bool active)
    {
        panelRoot.SetActive(active);
    }

    public void UpdateInventory(int grain, int flour, int bread)
    {
        grainText.text = $"Grain: {grain}";
        flourText.text = $"Flour: {flour}";
        breadText.text = $"Bread: {bread}";
    }
}