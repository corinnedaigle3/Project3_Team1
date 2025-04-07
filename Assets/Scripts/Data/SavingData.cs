using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameSaveData
{
    public string playerName;
    public string levelName;
}

public class SavingData : MonoBehaviour
{
    private string lastSavedLevelName;

    public string playerName = "DefaultPlayer";
    public string LevelNameNew;
    public Transform playerWorldPosition;

    //public CollectableManager manager;
    private string path;  // Declared at class level

    private string lastPlayerName;
    private string lastHighScore;

    void Awake()
    {
        path = Application.persistentDataPath + "/gamesave.json";  // ✅ Initialize early
    }

    void Start()
    {
        if (File.Exists(path))
        {
            LoadData(); // Load existing data

        }
        LevelNameNew = "MainHub";
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().name != "MainMenu")
        {
            LevelNameNew = SceneManager.GetActiveScene().name;
        }

        if (lastSavedLevelName != LevelNameNew)
        {
            SaveData();
        }
        

        if (Input.GetKeyDown(KeyCode.R)) // reset game data 
        {
            ResetData();
        }

    }

    void SaveData()
    {
        if (string.IsNullOrEmpty(path))  // ✅ Safety check
        {
            Debug.LogError("Save path is null or empty!");
            return;
        }

        GameSaveData saveData = new GameSaveData { playerName = playerName, levelName = LevelNameNew };
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(path, json);

        lastSavedLevelName = LevelNameNew;

        Debug.Log($"JSON Saved: {playerName}, {LevelNameNew}");
    }
    void ResetData()
    {
        GameSaveData saveData = new GameSaveData { playerName = playerName, levelName = null };
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(path, json);
    }

    void LoadData()
    {
        string jsonData = File.ReadAllText(path);
        GameSaveData loadedData = JsonUtility.FromJson<GameSaveData>(jsonData);
        playerName = loadedData.playerName;
        LevelNameNew = loadedData.levelName;
        lastSavedLevelName = loadedData.levelName;



        // Debug.Log($"JSON Loaded: {playerName}, {LevelNameNew}");

    }
}
