using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHubManager : MonoBehaviour
{
    public GameObject furyStatueE;
    public GameObject gemE;
    public GameObject furyStatueA;
    public GameObject gemA;
    public GameObject furyStatueT;
    public GameObject gemT;
    public GameObject tutorial;
    public GameObject tutorialPortal;

    private bool furySEPlaced1;
    private bool furySEPlaced2;
    private bool furySEPlaced3;
    private bool tutorialDone;

    public GameManger manager;
    PortalOutSFX portalOutSFX;

    void Start()
    {
        manager = FindObjectOfType<GameManger>();
        portalOutSFX = GameObject.FindGameObjectWithTag("PortalOut").GetComponent<PortalOutSFX>();
        furySEPlaced1 = false;
        furySEPlaced2 = false;
        furySEPlaced3 = false;
    }

    
    void Update()
    {
        tutorialDone = manager.tutorialDone;
        if (tutorialDone)
        {
            Debug.Log("Destroy tutorial");
            Destroy(tutorial);
            Destroy(tutorialPortal);
        }
        if (manager.furyA)
        {
            furyStatueA.SetActive(true);
            gemA.SetActive(true);
            furySEPlaced1 = true;
        }
            
        if (manager.furyT)
        {
            furyStatueT.SetActive(true); 
            gemT.SetActive(true);
            furySEPlaced2 = true;
        } 

        if (manager.furyE)
        {
            furyStatueE.SetActive(true); 
            gemE.SetActive(true);
            furySEPlaced3 = true;
        }

        if (furySEPlaced1 && furySEPlaced2 && furySEPlaced3)
        {
            portalOutSFX.portalOut1.UnPause();
            portalOutSFX.portalOut2.UnPause();
        }else
        {
            portalOutSFX.portalOut1.Pause();
            portalOutSFX.portalOut2.Pause();
        }
    }
}
