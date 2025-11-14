using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Cinemachine.PostFX;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class CinemachineVolumeBehavior : MonoBehaviour
{
    CinemachineVolumeSettings settings;   // Reference to the Cinemachine volume settings component
    string currentScene;                  // Holds the active scene name

    [Header("The volume Profiles for the Levels")]
    public VolumeProfile volumeTartarus;  // Profile for Tartarus scene
    public VolumeProfile volumeElysium;   // Profile for Elysium scene
    public VolumeProfile volumeAsphodel;  // Profile for Asphodel scene
    public VolumeProfile volumeMianHub;   // Profile for Main Hub scene

    // Start is called before the first frame update
    void Start()
    {
        settings = GetComponent<CinemachineVolumeSettings>(); // Get Cinemachine volume component
        currentScene = SceneManager.GetActiveScene().name;    // Get initial scene name
    }

    // Update is called once per frame
    void Update()
    {
        currentScene = SceneManager.GetActiveScene().name;    // Check scene every frame
        Debug.Log("current scene is: " +  currentScene);      // Debug the scene name

        switch (currentScene)                                 // Change profile based on scene name
        {
            case "MainHub":
                ChangeProfile(volumeMianHub);
                break;
            case "Elysium":
                ChangeProfile(volumeElysium);
                break;
            case "Tarturus":
                ChangeProfile(volumeTartarus);
                break;
            case "Asphodel":
                ChangeProfile(volumeAsphodel);
                break;
        }
    }

    void ChangeProfile(VolumeProfile v)
    {
        settings.m_Profile = v;    // Apply the chosen volume profile
    }
}
