using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using JetBrains.Annotations;

public class UI : MonoBehaviour
{
  
    private static UI instance;

    public string currentScene;
    public string goToScene;
    public bool isPaused;
    public GameObject pauseMenu;
    public GameManger gameManager;

    public TextMeshProUGUI txt;
   // public string textForPopUp;
    public GameObject popUpBar;
    public GameObject popUpBar2;


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
    public GameObject controlsInPauseFirst;
    public GameObject howToWinFirst;

    [Header("For Back buttons")]
    public GameObject menu;
    public GameObject controlsMenu;
    public GameObject creditsMenu;
    public GameObject menuInPause;
    public GameObject controlsInPause;
    public GameObject howToWin;
   


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
             //   EventSystem.current.SetSelectedGameObject(mainMenuFirst);

                mainMenu.SetActive(true);
                lose.SetActive(false);
                Levels.SetActive(false);
                win.SetActive(false);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case "Lose":
                // EventSystem.current.SetSelectedGameObject(loseFirst);
          

                mainMenu.SetActive(false);
                lose.SetActive(true);
                Levels.SetActive(false);
                win.SetActive(false);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case "LOSE":
                // EventSystem.current.SetSelectedGameObject(loseFirst);
               

                mainMenu.SetActive(false);
                lose.SetActive(true);
                Levels.SetActive(false);
                win.SetActive(false);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case "Win":
            //    EventSystem.current.SetSelectedGameObject(winFirst);

                mainMenu.SetActive(false);
                lose.SetActive(false);
                Levels.SetActive(false);
                win.SetActive(true);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break ;
            case "WIN":
            //    EventSystem.current.SetSelectedGameObject(winFirst);

                mainMenu.SetActive(false);
                lose.SetActive(false);
                Levels.SetActive(false);
                win.SetActive(true);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
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
        Destroy(GameObject.Find("NewPlayer"));
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
        SceneManager.LoadScene("WIN");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }  
    public void LoadLose()
    {
        gameManager.playerDisable();
        EventSystem.current.SetSelectedGameObject(loseFirst);

        SceneManager.LoadScene("Lose");
        SceneManager.LoadScene("LOSE");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("Cursor should be visible ");
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

    public void OpenControls()
    {
        BackButton(menu, controlsMenu, controlsFirst);
    }

    public void CloseControls()
    {
        BackButton(controlsMenu, menu, mainMenuFirst);

    }   
   
    public void OpenCredits()
    {
        BackButton(menu, creditsMenu, creditsFirst);
    }
    public void CloseCredits()
    {
        BackButton(creditsMenu, menu, mainMenuFirst);

    }

    // for pause menu
    public void POpenControls()
    {
        BackButton(menuInPause, controlsInPause, controlsInPauseFirst);
    }

    public void PCloseControls()
    {
        BackButton(controlsInPause, menuInPause, pauseFirst);

    }
    public void POpenHowToWin()
    {
        BackButton(menuInPause, howToWin, howToWinFirst);
    }
    public void PCloseHowToWin()
    {
        BackButton(howToWin, menuInPause, pauseFirst);

    }
    // function that opens and closes menus in UI
    public void BackButton(GameObject disable, GameObject enable, GameObject forEventSystem)
    {
        disable.SetActive(false);
        enable.SetActive(true);
        EventSystem.current.SetSelectedGameObject(forEventSystem);

    }

    public void ResetGame()
    {
       // gameManager.resetData();
        gameManager.resetData();
        LoadMain();
        gameManager.resetData();
        gameManager.resetData();
        gameManager.resetData();
        gameManager.resetData();
        gameManager.resetData();
       // Destroy(this.gameObject);


    }
}
