using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private string savePath;    // Store the path where my game will be saved

    private void Awake()
    {
        savePath = Application.persistentDataPath + "/player_save.json";
    }

    // Save Game
    public void SaveGame(Vector3 playerPos)
    {
        // Format our player position data to save to file.
        PlayerSaveData data = new PlayerSaveData();
        data.position[0] = playerPos.x;
        data.position[1] = playerPos.y;
        data.position[2] = playerPos.z;

        // 1. Save our data in a JSON format
        string json = JsonUtility.ToJson(data, true); // Optional 2nd argument. True makes the file readable
        // 2. Write to a JSON file
        File.WriteAllText(savePath, json);
        Debug.Log("Game Saved to: " + savePath);
    }

    // Load Game
    public Vector3 LoadGame()
    {
        if(File.Exists(savePath))
        {
            // File exists!
            string json = File.ReadAllText(savePath);   // Load the JSON string from savePath

            PlayerSaveData data = JsonUtility.FromJson<PlayerSaveData>(json); // Convert the JSON string into a PlayerSaveData
            Vector3 playerData = new Vector3(data.position[0], data.position[1], data.position[2]);
            return playerData;
        }

        Debug.LogWarning("No save file found at " + savePath);
        return Vector3.zero;
    }
}
