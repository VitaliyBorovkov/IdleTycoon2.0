using UnityEngine;

[CreateAssetMenu(fileName = "FarmSettingsDatabase", menuName = "ScriptableObjects/Farm/FarmSettingsDatabase", order = 2)]
public class FarmSettingsDatabase : ScriptableObject
{
    public FarmSettings[] levels;
}