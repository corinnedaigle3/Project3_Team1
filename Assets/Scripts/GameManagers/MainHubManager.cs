using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHubManager : MonoBehaviour
{
    public GameObject furyStatueE;
    public GameObject furyStatueA;
    public GameObject furyStatueT;

    public GameManger manager;
    
    void Start()
    {
        manager = FindObjectOfType<GameManger>();
    }

    
    void Update()
    {
        if (manager.furyA)
            furyStatueA.SetActive(true);
        if (manager.furyT)
            furyStatueT.SetActive(true); 
        if (manager.furyE)
            furyStatueA.SetActive(true);
    }
}
