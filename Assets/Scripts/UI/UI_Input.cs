using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UI_Input : MonoBehaviour
{
    private PlayerInput input;
    InputAction menuOpen;
    public UI ui;
    public bool MenuOpenClose { get; private set; }

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        menuOpen = input.actions["OpenPause"];
        ui = GetComponentInParent<UI>();
    }
    private void Update()
    {
        MenuOpenClose = menuOpen.WasPressedThisFrame();
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            // pauses and unpauses the game 
            if (!ui.isPaused && MenuOpenClose)
            {
                ui.PauseGame();
            }
            else if (ui.isPaused && MenuOpenClose)
            {
                ui.ResumeGame();
            }
        }
        
    }
}
