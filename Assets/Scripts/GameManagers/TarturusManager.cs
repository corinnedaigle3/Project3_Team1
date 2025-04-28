using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarturusManager : MonoBehaviour
{
    public GameObject[] fallingPillars;
    public bool playerFell = false;
    int i = 0;


    void Update()
    {
        if (playerFell)
        {
            i = 0;  
            foreach (GameObject pillar in fallingPillars)
            {
                i++;
                Debug.Log("Enable all the pillars again " + i);

                pillar.SetActive(true); // Re-enable the pillar
            }
            Debug.Log("Player fell is now " + playerFell);
            playerFell = false;
            Debug.Log("Player fell is now " + playerFell);
        }
    }
}
