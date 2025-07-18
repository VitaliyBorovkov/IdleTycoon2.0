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
            CurrentXP -= xpToNextLevel[CurrentLevel];
            CurrentLevel++;
            levelUp = true;
        }

        return levelUp;
    }
}