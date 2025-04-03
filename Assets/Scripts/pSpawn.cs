using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pSpawn : MonoBehaviour
{
    public GameObject player;
    bool playerIsHere = false;

    private void Awake()
    {
        player = GameObject.Find("Player");
        player.transform.position = transform.position;
    }
  

 
    private void FixedUpdate()
    {

        if (!playerIsHere)
        {
            playerIsHere = true;
            player.transform.position = transform.position;
        }
    }
   

}
