using UnityEngine;

[CreateAssetMenu(fileName = "MillerSettings", menuName = "ScriptableObjects/Mill/Miller Settings")]
public class MillerSettings : ScriptableObject
{
    public float moveSpeed = 2f;
    public float craftTime = 2f;
    public int grainPerBatch = 3;
    public int flourPerBatch = 2;
}