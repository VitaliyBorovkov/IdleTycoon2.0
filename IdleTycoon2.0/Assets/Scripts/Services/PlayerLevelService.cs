using System;

using UnityEngine;

public class PlayerLevelService : MonoBehaviour, IPlayerLevelService
{
    [SerializeField] private PlayerLevelSettings settings;

    public event Action<int> OnLevelUp;
    public event Action OnXPChanged;

    private PlayerLevelModel model;

    public int CurrentLevel => model.CurrentLevel;
    public int CurrentXP => model.CurrentXP;

    private void Awake()
    {
        model = new PlayerLevelModel(settings);
    }

    public void AddXP(int amount)
    {
        bool leveledUp = model.TryAddXP(amount, out bool levelUp);

        Debug.Log($"[PlayerLevel] Gained {amount} XP. Total XP: {CurrentXP}");

        OnXPChanged?.Invoke();

        if (levelUp)
        {
            Debug.Log($"[PlayerLevel] Leveled up! New level: {CurrentLevel}");
            OnLevelUp?.Invoke(CurrentLevel);
        }
    }

    public int XPToNextLevel()
    {
        return model.XPToNextLevel();
    }

    public int GetRequiredXPForNextLevel()
    {
        return model.XPToNextLevel();
    }

    public int RequiredXPForCurrentLevel()
    {
        return model.RequiredXPForCurrentLevel();
    }

    public int CurrentXPInLevel()
    {
        return model.CurrentXPInLevel();
    }

    public void SetXP(int xp)
    {
        model.SetXP(xp);
        OnXPChanged?.Invoke();
        Debug.Log($"[PlayerLevel] XP set to {xp}");
    }

    public void SetLevel(int level)
    {
        model.SetLevel(level);
        OnLevelUp?.Invoke(level);
        Debug.Log($"[PlayerLevel] Level set to {level}");
    }
}