using UnityEngine;

[CreateAssetMenu(fileName = "FarmerStatsDatabase", menuName = "ScriptableObjects/Farmer/FarmerStatsDatabase", order = 2)]
public class FarmerStatsDatabase : ScriptableObject
{
    public FarmerStats[] levels;
}