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
        if (player == null)
        {
            player = GameObject.Find("Player");
        }
        else
            Debug.Log("No Player Found");

        ui.goToScene = data.LevelNameNew;
        Debug.Log("Scene name " + ui.goToScene);


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
