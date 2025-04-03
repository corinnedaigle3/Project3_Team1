using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public GameObject credits;
    public GameObject start;
    public GameObject quit;
    public GameObject back;
    public GameObject resume;
    public GameObject mainMenu;
    public GameObject creditsMenu;
    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject pauseMenu;

    public InventoryManager inventoryManager;

    public bool levels;
    public bool main;
    public bool pause;
    public bool lose;
    public bool win;
    public static bool isPaused;
    public bool inventory;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //inventory manager
        inventory = false;

        //Buttons
        credits.gameObject.SetActive(true);
        start.gameObject.SetActive(true);
        quit.gameObject.SetActive(true);
        back.gameObject.SetActive(false);
        pauseMenu.SetActive(false);

        //Backgrounds for Main
        mainMenu.gameObject.SetActive(true);
        creditsMenu.gameObject.SetActive(false);

        //Backgound for Lose

        //background for Win
    }

    // Update is called once per frame
    void Update()
    {
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

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void BackToMenu()
    {
        credits.gameObject.SetActive(true);
        start.gameObject.SetActive(true);
        quit.gameObject.SetActive(true);
        back.gameObject.SetActive(false);

        mainMenu.gameObject.SetActive(true);
        creditsMenu.gameObject.SetActive(false);
    }


    public void Esc()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Credits()
    {
        credits.gameObject.SetActive(false);
        start.gameObject.SetActive(false);
        quit.gameObject.SetActive(false);
        back.gameObject.SetActive(true);

        mainMenu.gameObject.SetActive(false);
        creditsMenu.gameObject.SetActive(true);
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
