using UnityEngine;

public class XPBarController : MonoBehaviour
{
    [SerializeField] private XPBarView view;

    private PlayerLevelSystem levelSystem;

    public void Initialize(PlayerLevelSystem levelSystem)
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
        HandleXPChanged();
    }

    private void UpdateUI()
    {
        int totalXP = levelSystem.CurrentXP;
        int requiredXP = levelSystem.RequiredXPForCurrentLevel();
        //int currentXP = levelSystem.CurrentXPInLevel();
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