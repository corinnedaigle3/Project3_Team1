using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchBehave : MonoBehaviour
{
    public GameObject theCatch;
    public bool playerBehind =false;
    public PlayerMovement p;
    
    void Start()
    {
        p = GameObject.Find("Player").GetComponent<PlayerMovement>();
     
    }

    // Update is called once per frame
    void Update()
    {
        if (p.alreadyInvisible)
        {
            theCatch.SetActive(false);
          
        }
        else if (!p.alreadyInvisible)
        {
            theCatch.SetActive(true);
        }
    }


}
