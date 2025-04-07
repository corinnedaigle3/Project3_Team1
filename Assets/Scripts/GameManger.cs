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

    private void Awake()
    {
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
       
    
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) // look for player when player is not around
        {
            player = GameObject.Find("Player");
        }
        else
            Debug.Log("No Player Found");

        ui.goToScene = data.LevelNameNew;
        Debug.Log("Scene name " + ui.goToScene);

        // This is reseting the information so when you lose you have to start all over again from the sstart  
        if (player.GetComponent<PlayerMovement>().lose)
        {
            data.LoadFromSafeFile();
        }

    }

    public void playerEnable()
    {
        if (player == null)
        {
            player = GameObject.Find("Player");
        }else if (player != null)
        {
            player.SetActive(true);
        }
        else
            Debug.Log("No Player Found");

    }
    public void playerDisable()
    {
        if (player == null)
        {
            player = GameObject.Find("Player");

        }
        else if (player != null)
        {
            player.SetActive(false);
        }
        else
            Debug.Log("No Player Found");

    }

   
}
