using System;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class FarmUpgradeView : MonoBehaviour
{
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI costText;

    public event Action OnUpgradeClicked;

    public void Initialize()
    {
        upgradeButton.onClick.AddListener(() => OnUpgradeClicked?.Invoke());
    }

    public void SetButtonVisible(bool visible)
    {
        upgradeButton.gameObject.SetActive(visible);
    }

    public void SetButtonInteractable(bool interactable)
    {
        upgradeButton.interactable = interactable;
    }

    public void SetCost(int cost)
    {
        costText.text = $"Upgrade ({cost})";
    }
}