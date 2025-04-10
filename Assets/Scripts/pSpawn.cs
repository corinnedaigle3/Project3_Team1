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
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
       
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
