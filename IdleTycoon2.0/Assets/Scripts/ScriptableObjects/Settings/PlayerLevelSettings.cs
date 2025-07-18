using UnityEngine;

[CreateAssetMenu(fileName = "PlayerLevelSettings", menuName = "Settings/Player Level Settings")]
public class PlayerLevelSettings : ScriptableObject
{
    public int[] xpToNextLevel;
}