using UnityEngine;

public class XPBarController : MonoBehaviour
{
    [SerializeField] private XPBarView view;

    private IPlayerLevelService levelSystem;

    public void Initialize(IPlayerLevelService levelSystem)
    {
        this.levelSystem = levelSystem;

        levelSystem.OnXPChanged += HandleXPChanged;
        levelSystem.OnLevelUp += HandleLevelUp;

        UpdateUI();
    }

    private void HandleXPChanged()
    {
        UpdateUI();
    }

    private void HandleLevelUp(int newLevel)
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        int totalXP = levelSystem.CurrentXP;
        int requiredXP = levelSystem.RequiredXPForCurrentLevel();
        view.UpdateXPBar(totalXP, requiredXP);
    }

    private void OnDestroy()
    {
        if (levelSystem != null)
        {
            levelSystem.OnXPChanged -= HandleXPChanged;
            levelSystem.OnLevelUp -= HandleLevelUp;
        }
    }
}