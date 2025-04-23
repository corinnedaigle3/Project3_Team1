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
               
                pillar.SetActive(true); // Re-enable the pillar
            }
            playerFell = false;
            Debug.Log("Player Fell and all pillars restored");
        }
    }
}
