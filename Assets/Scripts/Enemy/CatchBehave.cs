using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchBehave : MonoBehaviour
{
    public GameObject theCatch;
   
    public PlayerMovement p;
    
    void Start()
    {
        theCatch = this.gameObject;
        p = GameObject.Find("Player").GetComponent<PlayerMovement>();
     
    }


    void Update()
    {
        GameObject.Find("Player");

        if (p.Invisible) // if player uses invisibility ability it can't catch player
        {
            theCatch.GetComponent<Collider>().enabled = false;
          
        }
        else if (!p.Invisible)
        {
            theCatch.GetComponent<Collider>().enabled = true;
        }
    }
}
