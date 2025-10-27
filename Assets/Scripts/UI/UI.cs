using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using JetBrains.Annotations;

// Handles all UI logic for the game:
// - Menu activation per scene
// - Pause / resume control
// - Scene transitions
// - Cursor visibility and navigation
// - Button navigation through EventSystem
public class UI : MonoBehaviour
{
    // Singleton instance for global UI access across scenes
    private static UI instance;

    // Tracks current and target scenes
    public string currentScene;
    public string goToScene;

    // Pause state
    public bool isPaused;

    // Reference to the pause menu object
    public GameObject pauseMenu;

    // Reference to the main GameManager (controls global systems)
    public GameManger gameManager;

    // Popup text display (for tooltips or notifications)
    public TextMeshProUGUI txt;
    public GameObject popUpBar;
    public GameObject popUpBar2;
    public GameObject inventory;

    [Header("Scene Based screen references ")]
    // Menus for different game states
    public GameObject mainMenu;
    public GameObject Levels;
    public GameObject win;
    public GameObject lose;

    // EventSystem reference for UI navigation
    public GameObject EventSystemM;

    [Header("First Selected UI Elements")]
    // Buttons or elements that should be selected first in each menu
    public GameObject mainMenuFirst;
    public GameObject loseFirst;
    public GameObject winFirst;
    public GameObject pauseFirst;
    public GameObject controlsFirst;
    public GameObject creditsFirst;
    public GameObject controlsInPauseFirst;
    public GameObject howToWinFirst;

    [Header("Menu Groups (for back buttons)")]
    // Used for showing/hiding submenus and returning to main menus
    public GameObject menu;
    public GameObject controlsMenu;
    public GameObject creditsMenu;
    public GameObject menuInPause;
    public GameObject controlsInPause;
    public GameObject howToWin;

    // Ensures only one UI instance persists across scene loads
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Prevent duplicate UI managers
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist through scene changes
        }
    }

    void Start()
    {
        // Set display resolution (optional, for consistent fullscreen)
        Screen.SetResolution(1920, 1080, true);

        // Make sure the cursor is visible in menus
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Get reference to the GameManager
        gameManager = GameObject.Find("GameManager").GetComponent<GameManger>();

        // Debug info and select the first menu option after a short delay
        Debug.Log("Selecting mainMenuFirst: " + mainMenuFirst);
        StartCoroutine(waitSomeTime(1f));
    }

    // Waits a short time before selecting the first UI element
    IEnumerator waitSomeTime(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        EventSystem.current.SetSelectedGameObject(mainMenuFirst);
    }

    void Update()
    {
        // Continuously track the current active scene
        currentScene = SceneManager.GetActiveScene().name;
        Debug.Log("Current Scene is " + currentScene);

        // Activate or deactivate UI elements depending on the scene
        switch (currentScene)
        {
            case "CutScene":
                mainMenu.SetActive(false);
                lose.SetActive(false);
                Levels.SetActive(true);
                win.SetActive(false);
                inventory.SetActive(false);
                break;

            case "MainMenu":
                mainMenu.SetActive(true);
                lose.SetActive(false);
                Levels.SetActive(false);
                win.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;

            case "Lose":
            case "LOSE": // Handles both naming conventions
                mainMenu.SetActive(false);
                lose.SetActive(true);
                Levels.SetActive(false);
                win.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;

            case "Win":
            case "WIN": // Handles both naming conventions
                mainMenu.SetActive(false);
                lose.SetActive(false);
                Levels.SetActive(false);
                win.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;

            default:
                // Default for gameplay scenes
                mainMenu.SetActive(false);
                lose.SetActive(false);
                Levels.SetActive(true);
                win.SetActive(false);
                inventory.SetActive(true);
                break;
        }

        // Quit the game when pressing Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    //Scene Loading Functions
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

    public void LoadHub()
    {
        SceneManager.LoadScene("MainHub");
        ResumeGame();
    }

    public void LoadWin()
    {
        gameManager.playerDisable();
        EventSystem.current.SetSelectedGameObject(winFirst);
        // Loads both "Win" and "WIN" scenes (likely a redundancy)
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

    // Pause Menu Functions
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f; // Freeze gameplay
        EventSystem.current.SetSelectedGameObject(pauseFirst);
        isPaused = true;

        // Unlock and show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; // Resume gameplay
        isPaused = false;

        // Lock and hide cursor for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Menu Navigation Functions
    public void OpenControls() => BackButton(menu, controlsMenu, controlsFirst);
    public void CloseControls() => BackButton(controlsMenu, menu, mainMenuFirst);
    public void OpenCredits() => BackButton(menu, creditsMenu, creditsFirst);
    public void CloseCredits() => BackButton(creditsMenu, menu, mainMenuFirst);

    // Pause menu versions
    public void POpenControls() => BackButton(menuInPause, controlsInPause, controlsInPauseFirst);
    public void PCloseControls() => BackButton(controlsInPause, menuInPause, pauseFirst);
    public void POpenHowToWin() => BackButton(menuInPause, howToWin, howToWinFirst);
    public void PCloseHowToWin() => BackButton(howToWin, menuInPause, pauseFirst);

    // Generic helper to disable one menu and enable another
    public void BackButton(GameObject disable, GameObject enable, GameObject forEventSystem)
    {
        disable.SetActive(false);
        enable.SetActive(true);
        EventSystem.current.SetSelectedGameObject(forEventSystem);
    }

    // Reset & Data Management
    public void ResetGame()
    {
        // Fully reset player progress and return to main menu
        gameManager.resetData();
        LoadMain();

        // Extra redundant calls (could be simplified)
        gameManager.resetData();
        gameManager.resetData();
        gameManager.resetData();
        gameManager.resetData();
        gameManager.resetData();
    }
}
