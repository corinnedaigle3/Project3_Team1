using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarturusManager : MonoBehaviour
{
    public GameObject[] fallingPillars;
    public bool playerFell;


    void Update()
    {
        if (playerFell)
        {
            foreach (GameObject pillar in fallingPillars)
            {
                Debug.Log("Enable all the pillars again");

                pillar.SetActive(true); // Re-enable the pillar
            }
            Debug.Log("Player fell is now " + playerFell);
            playerFell = false;
            Debug.Log("Player fell is now " + playerFell);
        }
    }
}
