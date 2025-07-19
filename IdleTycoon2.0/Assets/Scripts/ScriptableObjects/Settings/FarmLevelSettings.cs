using UnityEngine;

[CreateAssetMenu(fileName = "FarmLevelSettings", menuName = "Settings/Farm Level Settings")]
public class FarmLevelSettings : ScriptableObject
{
    public FarmLevelEntry[] levels;
}

[System.Serializable]
public class FarmLevelEntry
{
    public int level;
    public float productionInterval;
    public int grainPerCycle;
    public int buildCost;
    public int upgradeCost;
}