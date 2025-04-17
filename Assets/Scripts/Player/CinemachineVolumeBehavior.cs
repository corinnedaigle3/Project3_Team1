using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Cinemachine.PostFX;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;


public class CinemachineVolumeBehavior : MonoBehaviour
{
    CinemachineVolumeSettings settings;
    string currentScene;

    [Header("The volume Profiles for the Levels")]
    public VolumeProfile volumeTartarus;
    public VolumeProfile volumeElysium;
    public VolumeProfile volumeAsphodel;
    public VolumeProfile volumeMianHub;

    // Start is called before the first frame update
    void Start()
    {
        settings = GetComponent<CinemachineVolumeSettings>();
        currentScene = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        currentScene = SceneManager.GetActiveScene().name;
        switch (currentScene)
        {
            case "MainHub":
                ChangeProfile(volumeMianHub);
                break;
            case "Elysium":
                ChangeProfile(volumeElysium);
                break;
            case "Tartarus":
                ChangeProfile(volumeTartarus);
                break;
            case "Asphodel":
                ChangeProfile(volumeAsphodel);
                break;

        }
    }

    void ChangeProfile(VolumeProfile v)
    {
        settings.m_Profile = v;

    }
}
