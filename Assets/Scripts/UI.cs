using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    private static UI instance;

    public string goToScene;
    private bool isPaused;
    public GameObject pauseMenu;
    public GameManger gameManager;


    [Header("Scene Based screen references ")]
    public GameObject mainMenu;
    public GameObject Levels;


    // not sure yet 
   // public InventoryManager inventoryManager;

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


    void Start()
    {
        Screen.SetResolution(1920, 1080, true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManger>();


    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            mainMenu.SetActive(true);
            Levels.SetActive(false);
        }
        else
        {
            mainMenu.SetActive(false);
            Levels.SetActive(true);

            if (Input.GetKeyDown(KeyCode.P))
            {
                if (isPaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }
       
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

    public void Play()
    {
        ResumeGame();
        SceneManager.LoadScene(goToScene);
        gameManager.playerEnable();

    }

    public void LoadMain()
    {
        gameManager.playerDisable();
        SceneManager.LoadScene("MainMenu");

    }

    public void Quit()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        //Unlocks cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        // Locks cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
