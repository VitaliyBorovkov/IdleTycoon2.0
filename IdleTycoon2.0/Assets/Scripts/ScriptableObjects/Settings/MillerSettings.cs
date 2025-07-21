using UnityEngine;

[CreateAssetMenu(fileName = "MillerSettings", menuName = "ScriptableObjects/Mill/Miller Settings")]
public class MillerSettings : ScriptableObject
{
    public float moveSpeed = 2f;
    public int xpForDelivered = 25;
    public int moneyForDelivered = 1;
    public int flourCarryAmount = 2;
}