using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class gemsChecker : MonoBehaviour
{
    // The intention of this scripts is to check how many gems player have and enable 

    public InventoryManager playerInventory;
    public GameManger manger;
    public bool canPressQ = false;
    string gemCheckerName;
    public bool qPressed;

    TextMeshProUGUI popUp;
    public string popUpText = "Press 'Q' to use Gem ";
    public string whichGem;

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
        other.gameObject.GetComponent<PlayerMovement>().theGemChecker = gameObject.GetComponent<gemsChecker>();
        popUp.text = popUpText + whichGem;

        if (useEpopUp != null)
        {
            useEpopUp.SetActive(true);
        }

        canPressQ = true;
    }

    private void OnTriggerExit(Collider other)
    {
        canPressQ = false;
        popUp.text = null;

        if (useEpopUp != null)
        {
            useEpopUp.SetActive(false);
        }
        other.gameObject.GetComponent<PlayerMovement>().theGemChecker = null;

    }

    private void Update()
    {
        if (popUp == null)
        {
            popUp = GameObject.Find("PopUp").GetComponent<TextMeshProUGUI>();

        }
        switch (gemCheckerName)
        {
            case "GemCheckT":
                // need to add some kind of logic for pop up here 
                whichGem = " Tartarus Gem";

                // enables the statue if it is true
                if (playerInventory.gemCounterT == 1 && qPressed && canPressQ)
                {
                    manger.furyT = true; // when ture, fury on said level will not spawn anymore 
                    furyStatueT.SetActive(true);
                    playerInventory.gemCounterT--;
                    playerInventory.hasGemT = false;
                    playerInventory.ShowAmount(playerInventory.gemTextT, playerInventory.gemCounterT, ref playerInventory.hasGemT);
                }
                break; 
            case "GemCheckE":
                // need to add some kind of logic for pop up here 
                whichGem = " Elysium Gem";
                // enables the statue if it is true
                if (playerInventory.gemCounterE == 1 && qPressed && canPressQ)
                {
                    manger.furyE = true;
                    furyStatueE.SetActive(true);
                    playerInventory.gemCounterE--;
                    playerInventory.hasGemE = false;
                    playerInventory.ShowAmount(playerInventory.gemTextE, playerInventory.gemCounterE, ref playerInventory.hasGemE);
                }
                break;
            case "GemCheckA":

                whichGem = " Asphodel Gem";

                if (playerInventory.gemCounterA == 1 && qPressed && canPressQ)
                {
                    manger.furyA = true; // when ture, fury on said level will not spawn anymore 

                    furyStatueA.SetActive(true);

                    playerInventory.gemCounterA--;
                    playerInventory.hasGemA = false;
                    playerInventory.ShowAmount(playerInventory.gemTextA, playerInventory.gemCounterA, ref playerInventory.hasGemA);
                }
                break;
            default:
                break;
            }
        }
    }



