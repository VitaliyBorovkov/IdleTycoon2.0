using UnityEngine;

public class FarmView : MonoBehaviour
{
    [SerializeField] private Transform farmerSpawnPoint;

    public Transform GetBotSpawnPoint()
    {
        return farmerSpawnPoint;
    }
}