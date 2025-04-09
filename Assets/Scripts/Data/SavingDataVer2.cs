using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[System.Serializable]
public class LevelObjectState2
{
    public string objectId;
    public Vector3 position;
    public Quaternion rotation;
    public bool isActive;
}

[System.Serializable]
public class GameSaveData2
{
    // Inventory data
    public string playerName;
    public string levelName;
    public bool hasApple;
    public bool hasSkull;
    public bool hasFireFlower;
    public bool hasGemE;
    public bool hasGemA;
    public bool hasGemT;
    public bool hasHelm;
    public int TakeDownItemEcounter;
    public int TakeDownItemAcounter;
    public int TakeDownItemTcounter;
    public int GemEcounter;
    public int GemAcounter;
    public int GemTcounter;
    public int helmcounter;

    // Level state data
    public List<string> savedLevelNames = new List<string>();
    public List<List<LevelObjectState>> savedLevelStates = new List<List<LevelObjectState>>();

    public void SaveLevelState(string levelName, List<LevelObjectState> state)
    {
        int index = savedLevelNames.IndexOf(levelName);
        if (index >= 0)
        {
            savedLevelStates[index] = state;
        }
        else
        {
            savedLevelNames.Add(levelName);
            savedLevelStates.Add(state);
        }
    }

    public List<LevelObjectState> LoadLevelState(string levelName)
    {
        int index = savedLevelNames.IndexOf(levelName);
        return index >= 0 ? savedLevelStates[index] : null;
    }
}

public class SavingDataVer2 : MonoBehaviour
{
    public InventoryManager inventoryManager;
    private string lastSavedLevelName;
    public string playerName = "DefaultPlayer";
    public string LevelNameNew;

    private string path;
    private string safeFilePath;

    void Awake()
    {
        path = Application.persistentDataPath + "/gamesave.json";
        safeFilePath = Application.persistentDataPath + "/safeStart.json";
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    void Start()
    {
        if (File.Exists(path))
        {
            LoadData();
        }
        else
        {
            LevelNameNew = "MainHub";
        }
        SaveSafeStart();
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            LevelNameNew = SceneManager.GetActiveScene().name;
        }

        if (lastSavedLevelName != LevelNameNew)
        {
            SaveData();
        }
    }

    void SaveData()
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("Save path is null or empty!");
            return;
        }

        // Create save data
        GameSaveData saveData = new GameSaveData
        {
            playerName = playerName,
            levelName = LevelNameNew,
            hasApple = inventoryManager.hasE,
            hasSkull = inventoryManager.hasA,
            hasFireFlower = inventoryManager.hasT,
            hasGemE = inventoryManager.hasGemE,
            hasGemA = inventoryManager.hasGemA,
            hasGemT = inventoryManager.hasGemT,
            hasHelm = inventoryManager.hasHelm,
            TakeDownItemEcounter = inventoryManager.takeDownItemCounterE,
            TakeDownItemAcounter = inventoryManager.takeDownItemCounterA,
            TakeDownItemTcounter = inventoryManager.takeDownItemCounterT,
            GemEcounter = inventoryManager.gemCounterE,
            GemAcounter = inventoryManager.gemCounterA,
            GemTcounter = inventoryManager.gemCounterT,
            helmcounter = inventoryManager.helmcounter
        };

        // Save level state
        var saveableObjects = FindObjectsOfType<SavableObjects>();
        var levelState = new List<LevelObjectState>();
        foreach (var obj in saveableObjects)
        {
            levelState.Add(new LevelObjectState
            {
                objectId = obj.objectId,
                position = obj.transform.position,
                rotation = obj.transform.rotation,
                isActive = obj.gameObject.activeSelf
            });
        }
        saveData.SaveLevelState(LevelNameNew, levelState);

        // Save to file
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(path, json);

        lastSavedLevelName = LevelNameNew;
        Debug.Log($"Game saved: {playerName}, {LevelNameNew}");
    }

    void LoadData()
    {
        string jsonData = File.ReadAllText(path);
        GameSaveData loadedData = JsonUtility.FromJson<GameSaveData>(jsonData);

        // Load inventory
        playerName = loadedData.playerName;
        LevelNameNew = loadedData.levelName;
        lastSavedLevelName = loadedData.levelName;

        inventoryManager.hasE = loadedData.hasApple;
        inventoryManager.hasA = loadedData.hasSkull;
        inventoryManager.hasT = loadedData.hasFireFlower;
        inventoryManager.hasGemE = loadedData.hasGemE;
        inventoryManager.hasGemA = loadedData.hasGemA;
        inventoryManager.hasGemT = loadedData.hasGemT;
        inventoryManager.hasHelm = loadedData.hasHelm;
        inventoryManager.takeDownItemCounterE = loadedData.TakeDownItemEcounter;
        inventoryManager.takeDownItemCounterA = loadedData.TakeDownItemAcounter;
        inventoryManager.takeDownItemCounterT = loadedData.TakeDownItemTcounter;
        inventoryManager.gemCounterE = loadedData.GemEcounter;
        inventoryManager.gemCounterA = loadedData.GemAcounter;
        inventoryManager.gemCounterT = loadedData.GemTcounter;
        inventoryManager.helmcounter = loadedData.helmcounter;

        // Load level state
        var levelState = loadedData.LoadLevelState(loadedData.levelName);
        if (levelState != null)
        {
            var saveableObjects = FindObjectsOfType<SavableObjects>();
            foreach (var obj in saveableObjects)
            {
                var savedState = levelState.Find(s => s.objectId == obj.objectId);
                if (savedState != null)
                {
                    obj.transform.position = savedState.position;
                    obj.transform.rotation = savedState.rotation;
                    obj.gameObject.SetActive(savedState.isActive);
                }
            }
        }

        Debug.Log($"Game loaded: {playerName}, {LevelNameNew}");
    }

    public void SaveSafeStart()
    {
        GameSaveData safeData = new GameSaveData
        {
            playerName = playerName,
            levelName = "MainHub",
            hasApple = false,
            hasSkull = false,
            hasFireFlower = false,
            hasGemE = false,
            hasGemA = false,
            hasGemT = false,
            hasHelm = false,
            TakeDownItemEcounter = 0,
            TakeDownItemAcounter = 0,
            TakeDownItemTcounter = 0,
            GemEcounter = 0,
            GemAcounter = 0,
            GemTcounter = 0,
            helmcounter = 0
        };

        string json = JsonUtility.ToJson(safeData);
        File.WriteAllText(safeFilePath, json);
        Debug.Log("Safe start file created.");
    }

    public void LoadFromSafeFile()
    {
        if (!File.Exists(safeFilePath))
        {
            Debug.LogError("Safe file not found!");
            return;
        }

        string jsonData = File.ReadAllText(safeFilePath);
        GameSaveData loadedData = JsonUtility.FromJson<GameSaveData>(jsonData);

        // Load safe data
        playerName = loadedData.playerName;
        LevelNameNew = loadedData.levelName;
        lastSavedLevelName = loadedData.levelName;

        if (inventoryManager == null)
            inventoryManager = FindObjectOfType<InventoryManager>();

        inventoryManager.hasE = loadedData.hasApple;
        inventoryManager.hasA = loadedData.hasSkull;
        inventoryManager.hasT = loadedData.hasFireFlower;
        inventoryManager.hasGemE = loadedData.hasGemE;
        inventoryManager.hasGemA = loadedData.hasGemA;
        inventoryManager.hasGemT = loadedData.hasGemT;
        inventoryManager.hasHelm = loadedData.hasHelm;
        inventoryManager.takeDownItemCounterE = loadedData.TakeDownItemEcounter;
        inventoryManager.takeDownItemCounterA = loadedData.TakeDownItemAcounter;
        inventoryManager.takeDownItemCounterT = loadedData.TakeDownItemTcounter;
        inventoryManager.gemCounterE = loadedData.GemEcounter;
        inventoryManager.gemCounterA = loadedData.GemAcounter;
        inventoryManager.gemCounterT = loadedData.GemTcounter;
        inventoryManager.helmcounter = loadedData.helmcounter;

        Debug.Log("Game reset to safe file.");
    }
}