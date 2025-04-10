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

    public string currentScene;
    public string goToScene;
    public bool isPaused;
    public GameObject pauseMenu;
    public GameManger gameManager;

    public TextMeshProUGUI txt;
    public string textForPopUp;


    [Header("Scene Based screen references ")]
    public GameObject mainMenu;
    public GameObject Levels;
    public GameObject win;
    public GameObject lose;

    public GameObject EventSystemM;
    [Header("First For each menu")]
    public GameObject mainMenuFirst;
    public GameObject loseFirst;
    public GameObject winFirst;
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
        currentScene = SceneManager.GetActiveScene().name;
        Debug.Log("Current Scene is " + currentScene);
        switch (currentScene)
        {
            case "MainMenu":
                mainMenu.SetActive(true);
                lose.SetActive(false);
                Levels.SetActive(false);
                win.SetActive(false);
                break;
            case "Lose":
               // EventSystem.current.SetSelectedGameObject(loseFirst);

                mainMenu.SetActive(false);
                lose.SetActive(true);
                Levels.SetActive(false);
                win.SetActive(false);
                break;
            case "LOSE":
               // EventSystem.current.SetSelectedGameObject(loseFirst);

                mainMenu.SetActive(false);
                lose.SetActive(true);
                Levels.SetActive(false);
                win.SetActive(false);
                break;
            case "Win":
            //    EventSystem.current.SetSelectedGameObject(winFirst);

                mainMenu.SetActive(false);
                lose.SetActive(false);
                Levels.SetActive(false);
                win.SetActive(true);
                break ;
            default:
                mainMenu.SetActive(false);
                lose.SetActive(false);
                Levels.SetActive(true);
                win.SetActive(false);
                break;

        }
        /*
       if(SceneManager.GetActiveScene().name == "MainMenu")
       {

       }
       else
       {
           mainMenu.SetActive(false);
           Levels.SetActive(true);


           // Moved this logic to the UI_Input script


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
        */

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

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    public void LoadWin()
    {
        gameManager.playerDisable();
        EventSystem.current.SetSelectedGameObject(winFirst);

        SceneManager.LoadScene("Win");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }  
    public void LoadLose()
    {
        gameManager.playerDisable();
        EventSystem.current.SetSelectedGameObject(loseFirst);

        SceneManager.LoadScene("Lose");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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

    public void ResetGame()
    {
        gameManager.resetData();
        LoadMain();
        gameManager.resetData();

    }
}
