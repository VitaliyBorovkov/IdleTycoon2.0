using UnityEngine;

[CreateAssetMenu(fileName = "BakerSettings", menuName = "ScriptableObjects/Bakery/Baker Settings")]
public class BakerSettings : ScriptableObject
{
    public float moveSpeed = 2f;
    public float craftTime = 3f;
    public int flourPerBatch = 2;
    public int breadPerBatch = 1;
}