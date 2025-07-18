using System;

using UnityEngine;

public class PlayerLevelSystem : MonoBehaviour
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
        // later можно перенести в модель
        return 0; // пока не используется
    }

    public void SetLevel(int level, int xp)
    {
        // только если будешь загружать из сейва
    }
}