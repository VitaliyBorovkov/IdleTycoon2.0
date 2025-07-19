using TMPro;

using UnityEngine;

public class PlayerLevelView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;

    public void SetLevel(int level)
    {
        levelText.text = $"Level: {level}";
    }
}