using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class gemsChecker : MonoBehaviour
{
    // The intention of this scripts is to check how many gems player have and enable 

    public GameObject Player;
    private bool canPressE = false;

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
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("It IS COLLIDING ");

        if (useEpopUp != null)
        {
            useEpopUp.SetActive(true);
        }

        canPressE = true;


       

    }

    private void OnTriggerExit(Collider other)
    {
        canPressE = false;
        if (useEpopUp != null)
        {
            useEpopUp.SetActive(false);
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canPressE)
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



}
