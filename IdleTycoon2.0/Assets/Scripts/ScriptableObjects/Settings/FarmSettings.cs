using UnityEngine;

[CreateAssetMenu(fileName = "FarmSettings", menuName = "ScriptableObjects/Farm/FarmSettings", order = 1)]

public class FarmSettings : ScriptableObject
{
    [Header("Build")]
    public int BuildCost;
    public int RequiredPlayerLevelForBuild;

    [Header("Upgrade")]
    public int UpgradeCost;
    public int RequiredPlayerLevelForUpgrade;

    public int MaxFarmLevel = 3;
}