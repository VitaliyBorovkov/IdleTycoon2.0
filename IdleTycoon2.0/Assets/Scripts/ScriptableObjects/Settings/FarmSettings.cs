using UnityEngine;

[CreateAssetMenu(fileName = "FarmData", menuName = "ScriptableObjects/Farm/FarmData", order = 1)]
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