using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class gemsChecker : MonoBehaviour
{
    // The intention of this scripts is to check how many gems player have and enable 

    public InventoryManager playerInventory;
    public GameManger manger;
    private bool canPressQ = false;
    string gemCheckerName;

    [Header("UI pop up")]
    public GameObject useEpopUp;

    [Header("Statues")]
    public GameObject furyStatueE;
    public GameObject furyStatueA;
    public GameObject furyStatueT;

    private void Start()
    {
        playerInventory = GameObject.Find("Canvas").GetComponent<InventoryManager>();
        gemCheckerName = gameObject.name;
        manger =  GameObject.Find("GameManager").GetComponent<GameManger>();
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("It IS COLLIDING ");

        if (useEpopUp != null)
        {
            useEpopUp.SetActive(true);
        }

        canPressQ = true;
    }

    private void OnTriggerExit(Collider other)
    {
        canPressQ = false;
        if (useEpopUp != null)
        {
            useEpopUp.SetActive(false);
        }

    }

    private void Update()
    {
        switch (gemCheckerName)
        {
            case "GemCheckT":
                // need to add some kind of logic for pop up here 

                // enables the statue if it is true
                if (playerInventory.GemTcounter == 1 && Input.GetKeyDown(KeyCode.Q) && canPressQ)
                {
                    manger.furyT = true; // when ture, fury on said level will not spawn anymore 
                    furyStatueT.SetActive(true);
                }
                break; 
            case "GemCheckE":
                // need to add some kind of logic for pop up here 

                // enables the statue if it is true
                if (playerInventory.GemEcounter == 1 && Input.GetKeyDown(KeyCode.Q) && canPressQ)
                {
                    manger.furyE = true;
                    furyStatueE.SetActive(true);
                }
                break;
            case "GemCheckA":
                if (playerInventory.GemAcounter == 1 && Input.GetKeyDown(KeyCode.Q) && canPressQ)
                {
                    manger.furyA = true; // when ture, fury on said level will not spawn anymore 

                    furyStatueA.SetActive(true);
                }
                break;
            default:
                break;
            }
        }
    }



