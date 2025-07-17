using UnityEngine;

[CreateAssetMenu(fileName = "FarmLevelSettings", menuName = "Settings/Farm Level Settings")]
public class FarmLevelSettings : ScriptableObject
{
    public int level = 1;
    public float productionInterval = 2f;
    public int grainPerCycle = 1;
}