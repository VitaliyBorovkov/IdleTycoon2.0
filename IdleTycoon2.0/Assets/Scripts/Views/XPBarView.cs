using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class XPBarView : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI xpText;

    public void UpdateXPBar(int currentXP, int requiredXP)
    {
        float progress = requiredXP > 0 ? (float)currentXP / requiredXP : 0;
        fillImage.fillAmount = Mathf.Clamp01(progress);
        xpText.text = $"XP: {currentXP} / {requiredXP}";
    }
}