using System.IO;

using UnityEngine;

public class SaveService
{
    private const string SaveFileName = "savegame.json";
    private string SaveFilePath => Path.Combine(Application.persistentDataPath, SaveFileName);

    public void SaveGame(SaveData saveData)
    {
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(SaveFilePath, json);
        Debug.Log($"[SaveService] Game saved to: {SaveFilePath}");
    }

    public SaveData LoadGame()
    {
        if (!File.Exists(SaveFilePath))
        {
            Debug.LogWarning("[SaveService] No save file found. Returning null.");
            return null;
        }

        string json = File.ReadAllText(SaveFilePath);
        SaveData saveData = JsonUtility.FromJson<SaveData>(json);
        return saveData;
    }
}