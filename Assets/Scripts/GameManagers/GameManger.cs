using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManger : MonoBehaviour
{
    private static GameManger instance;

    public SavingData data;
    public GameObject player;

    public string currentScene;
    public UI ui;

    public bool portalUsed;
    public GameObject spawnPoint;

    [Header("ConsumedG Gems")]
    public bool furyE;
    public bool furyA;
    public bool furyT;
    public bool win;
    public bool tutorialDone;

    private void Awake()
    {
        player = GameObject.Find("Player");
        
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instance
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        data = GetComponent<SavingData>();
        ui = GameObject.Find("Canvas").GetComponent<UI>();
        //player.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) // look for player when player is not around
        {
            player = GameObject.Find("Player");
        }
    

        if (data != null)
        {

        ui.goToScene = data.LevelNameNew;
        }
        Debug.Log("Scene name " + ui.goToScene);

        // This is reseting the information so when you lose you have to start all over again from the sstart  
        if (player != null && player.GetComponent<PlayerMovement>().lose)
        {
            data.LoadFromSafeFile();
        }

        if (furyA && furyE && furyT)
        {
            win = true;
        } 
        else
        {
            win = false;
        }

        
    }

    public void playerEnable()  // self explanatory 
    {
      if (player != null)
            player.SetActive(true);
        

    }
    public void playerDisable()
    {
        if (player == null)
        {
            player = GameObject.Find("Player");
           // player.SetActive(false);


        }
         if (player != null)
        {
            player.SetActive(false);
        }
        else
            Debug.Log("No Player Found");

    }

    public void resetData() // can be called 
    {
        data.LoadFromSafeFile();
        Debug.Log("Loading Reset Data");
    }

   
}
