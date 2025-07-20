using UnityEngine;

[CreateAssetMenu(fileName = "BakerySettings", menuName = "ScriptableObjects/Bakery/Bakery Settings")]
public class BakerySettings : ScriptableObject
{
    public int flourPerBatch = 2;
    public int breadPerBatch = 1;
    public float craftTime = 3f;
}