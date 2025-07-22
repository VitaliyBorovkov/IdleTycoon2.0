public class PlayerLevelModel
{
    public int CurrentLevel { get; private set; }
    public int CurrentXP { get; private set; }



    private int[] xpToNextLevel;

    public PlayerLevelModel(PlayerLevelSettings settings)
    {
        CurrentLevel = 1;
        CurrentXP = 0;
        xpToNextLevel = settings.xpToNextLevel;
    }


    public bool TryAddXP(int amount, out bool levelUp)
    {
        CurrentXP += amount;
        levelUp = false;

        while (CurrentLevel < xpToNextLevel.Length &&
               CurrentXP >= xpToNextLevel[CurrentLevel])
        {
            CurrentLevel++;
            levelUp = true;
        }

        return levelUp;
    }

    public int XPToNextLevel()
    {
        if (IsMaxLevel())
            return 0;

        return xpToNextLevel[CurrentLevel] - xpToNextLevel[CurrentLevel - 1];
    }

    public int CurrentXPInLevel()
    {
        if (CurrentLevel == 0)
            return CurrentXP;

        return CurrentXP - xpToNextLevel[CurrentLevel - 1];
    }

    public void SetLevel(int level)
    {
        CurrentLevel = level;
    }

    public void SetXP(int xp)
    {
        CurrentXP = xp;
    }

    public bool IsMaxLevel()
    {
        return CurrentLevel >= xpToNextLevel.Length;
    }

    public int RequiredXPForCurrentLevel()
    {
        if (CurrentLevel >= xpToNextLevel.Length)
            return xpToNextLevel[xpToNextLevel.Length - 1];

        return xpToNextLevel[CurrentLevel];
    }
}