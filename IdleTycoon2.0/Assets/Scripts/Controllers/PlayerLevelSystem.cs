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

    private void Start()
    {
        // Временно устанавливаем 95 XP на 1 уровне
        model.SetLevel(1, 95);
        Debug.Log($"[PlayerLevel] TEMP: XP manually set to {CurrentXP} at Level {CurrentLevel}");
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
}