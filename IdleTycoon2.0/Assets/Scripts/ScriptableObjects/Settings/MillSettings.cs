using UnityEngine;

[CreateAssetMenu(fileName = "MillSettings", menuName = "ScriptableObjects/Mill Settings")]
public class MillSettings : ScriptableObject
{
    public int grainPerBatch = 3;
    public int flourPerBatch = 2;
    public float craftTime = 2f;
}