using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameSaveData
{
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
}

public class SavingData : MonoBehaviour
{
    public InventoryManager inventoryManager;
    private string lastSavedLevelName;

    public string playerName = "DefaultPlayer";
    public string LevelNameNew;
   // public Transform playerWorldPosition; 

    //public CollectableManager manager;
    private string path;  // Declared at class level
    private string safeFilePath; // FileSafe path 

    private string lastPlayerName;
    private string lastHighScore;

    void Awake()
    {
        path = Application.persistentDataPath + "/gamesave.json";  //Initialize early
        safeFilePath = Application.persistentDataPath + "/safeStart.json";
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    void Start()
    {
        if (File.Exists(path))
        {
            LoadData(); // Load existing data

        }else
        {
            LevelNameNew = "MainHub";

        }
        SaveSafeStart();
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
     
    }

    void SaveData()
    {
        if (string.IsNullOrEmpty(path))  // ✅ Safety check
        {
            Debug.LogError("Save path is null or empty!");
            return;
        }

        GameSaveData saveData = new GameSaveData
        {
            playerName = playerName,
            levelName = LevelNameNew,

            // save all the information from inventroy manager
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
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(path, json);

        lastSavedLevelName = LevelNameNew;

        Debug.Log($"JSON Saved: {playerName}, {LevelNameNew}");
    }
    void LoadData()
    {
        string jsonData = File.ReadAllText(path);
        GameSaveData loadedData = JsonUtility.FromJson<GameSaveData>(jsonData);
        playerName = loadedData.playerName;
        LevelNameNew = loadedData.levelName;
        lastSavedLevelName = loadedData.levelName;

        inventoryManager = FindObjectOfType<InventoryManager>();

        // load all the inventory information saved 
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

        SaveData();
        // Debug.Log($"JSON Loaded: {playerName}, {LevelNameNew}");

    }
    public void SaveSafeStart()
    {
        GameSaveData safeData = new GameSaveData
        {
            playerName = playerName,
            levelName = "MainHub", // or whatever your start level is

            // Inventory base state (reset)
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

        SaveData();
        Debug.Log("Game reset to safe file.");
    }
}
