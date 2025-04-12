using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persistance : MonoBehaviour
{
    public static GameObject playerInstance;

    void Start()
    {
        if (playerInstance != null && playerInstance != this.gameObject)
        {
            Destroy(gameObject); // Destroy duplicate instance
        }
        else
        {
            playerInstance = this.gameObject;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
    }


}
