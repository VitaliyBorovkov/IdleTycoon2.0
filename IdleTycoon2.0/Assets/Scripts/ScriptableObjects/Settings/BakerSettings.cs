using UnityEngine;

[CreateAssetMenu(fileName = "BakerSettings", menuName = "ScriptableObjects/Bakery/Baker Settings")]
public class BakerSettings : ScriptableObject
{
    public float moveSpeed = 2f;
    public int xpForDelivered = 50;
    public int moneyForDelivered = 10;
    public int breadCarryAmount = 1;
}