using UnityEngine;

[CreateAssetMenu(fileName = "FarmSettings", menuName = "ScriptableObjects/Farm/FarmSettings", order = 1)]
public class FarmSettings : ScriptableObject
{
    [Header("Build")]
    public int BuildCost;
    public int RequiredPlayerLevelForBuild;

    [Header("Upgrade Levels")]
    public FarmLevelSettings[] levels;

    public int MaxFarmLevel => levels.Length;

    public FarmLevelSettings GetSettingsForLevel(int level)
    {
        if (level < 1 || level > levels.Length)
        {
            Debug.LogError($"[FarmSettings] Invalid level {level}");
            return null;
        }

        return levels[level - 1];
    }

    public FarmLevelSettings GetNextLevelSettings(int currentLevel)
    {
        //return GetSettingsForLevel(currentLevel + 1);
        if (currentLevel >= levels.Length)
        {
            Debug.LogWarning("[FarmSettings] No upgrade settings for current level.");
            return null;
        }

        return levels[currentLevel];
    }
}