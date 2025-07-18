using UnityEngine;

[CreateAssetMenu(fileName = "FarmerStats", menuName = "ScriptableObjects/Farmer/FarmerStats", order = 1)]
public class FarmerStats : ScriptableObject
{
    public int level;
    public float moveSpeed = 2f;
    public float harvestTime = 2f;
    public int grainPerHarvest = 1;
}