using TMPro;

using UnityEngine;

public class MoneyView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;

    public void UpdateMoney(int amount)
    {
        moneyText.text = $"Money: {amount}";
    }
}