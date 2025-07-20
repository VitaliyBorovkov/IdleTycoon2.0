using System;

public interface IPlayerLevelService
{
    event Action<int> OnLevelUp;
    event Action OnXPChanged;

    int CurrentLevel { get; }
    int CurrentXP { get; }

    void AddXP(int amount);
    int XPToNextLevel();
    int GetRequiredXPForNextLevel();
    int RequiredXPForCurrentLevel();
    int CurrentXPInLevel();
}