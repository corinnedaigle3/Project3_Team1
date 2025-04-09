using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UI : MonoBehaviour
{
  
    private static UI instance;

    public string goToScene;
    public bool isPaused;
    public GameObject pauseMenu;
    public GameManger gameManager;

    public TextMeshProUGUI txt;
    public string textForPopUp;


    [Header("Scene Based screen references ")]
    public GameObject mainMenu;
    public GameObject Levels;

    public GameObject EventSystemM;
    [Header("First For each menu")]
    public GameObject mainMenuFirst;
    public GameObject pauseFirst;
    public GameObject controlsFirst;
    public GameObject creditsFirst;


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


        Debug.Log("Selecting mainMenuFirst: " + mainMenuFirst);
        StartCoroutine(waitSomeTime(1f));
    }
    IEnumerator waitSomeTime(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        EventSystem.current.SetSelectedGameObject(mainMenuFirst);

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


            // Moved this logic to the UI_Input script

            /*
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
            }*/
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
        EventSystem.current.SetSelectedGameObject(mainMenuFirst);

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
        EventSystem.current.SetSelectedGameObject(pauseFirst);
        isPaused = true;

        //Unlocks cursor
        // Cursor.lockState = CursorLockMode.None;
        //  Cursor.visible = true;



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

    public void ResetGame()
    {
        gameManager.resetData();
        LoadMain();
        gameManager.resetData();

    }
}
