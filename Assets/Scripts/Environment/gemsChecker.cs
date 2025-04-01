using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class gemsChecker : MonoBehaviour
{
    // The intention of this scripts is to check how many gems player have and enable 

    public GameObject Player;

    [Header("UI pop up")]
    public GameObject useEpopUp;

    [Header("Statues")]
    public GameObject furyStatueE;
    public GameObject furyStatueA;
    public GameObject furyStatueT;

    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("It IS COLLIDING ");
        
        if(useEpopUp != null)
            useEpopUp.SetActive(true);

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E is being pressed");
            if (Player.GetComponent<PlayerMovement>().gemE == 1 && !furyStatueE.activeSelf)
            {
                furyStatueE.SetActive(true);
                Debug.Log(" Statue E is being showcased");

            }
            if (Player.GetComponent<PlayerMovement>().gemA == 1 && !furyStatueA.activeSelf)
            {
                furyStatueA.SetActive(true);

            }
            if (Player.GetComponent<PlayerMovement>().gemT == 1 && !furyStatueT.activeSelf)
            {
                furyStatueT.SetActive(true);

            }

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        useEpopUp.SetActive(false);

    }
    

}
