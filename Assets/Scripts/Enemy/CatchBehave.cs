using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchBehave : MonoBehaviour
{
    public GameObject theCatch;
    //public bool playerBehind =false;
    public PlayerMovement p;
    
    void Start()
    {
        theCatch = this.gameObject;
        p = GameObject.Find("Player").GetComponent<PlayerMovement>();
     
    }

    // Update is called once per frame
    void Update()
    {
        GameObject.Find("Player");

        if (p.Invisible)
        {
            theCatch.GetComponent<Collider>().enabled = false;
          
        }
        else if (!p.Invisible)
        {
            theCatch.GetComponent<Collider>().enabled = true;

           
        }
    }


}
