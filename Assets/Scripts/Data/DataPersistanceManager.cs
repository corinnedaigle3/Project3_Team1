using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DataPersistanceManager : MonoBehaviour
{
    private GameData gameData;

    public static DataPersistanceManager instance { get; private set;}

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager in the scene.");
        }

        instance = this;
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }
    public void LoadGame()
    {
        // to do - load game data
        // if no game data found, initialize new game
        if (this.gameData == null)
        {
            NewGame();
        } 
    }

    public void SaveGame() 
    { 
        // to do - pass data to other scripts to save data
        
        // to do - save data to a file using data handler

    }

}
