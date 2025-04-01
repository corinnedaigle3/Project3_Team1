using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class gemsChecker : MonoBehaviour
{
    // The intention of this scripts is to check how many gems player have and enable 

    public GameObject Player;       

    public GameObject furyStatue1;
    public GameObject furyStatue2;
    public GameObject furyStatue3;

    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (Player.GetComponent<PlayerMovement>().gems)
        {
            case 1:
                break;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }

    void Update()
    {
        
    }
}
