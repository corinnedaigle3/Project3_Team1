using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject spriteApple;
    public GameObject spriteSkull;
    public GameObject spriteFireFlower;
    public GameObject spriteGemT;
    public GameObject spriteGemA;
    public GameObject spriteGemE;
    public GameObject takeDowntext;
    public GameObject invisText;
    public GameObject dodgeText;
    public GameObject helmUseText;

    public GameObject spriteHelm;
    public TextMeshProUGUI helmText;
    public TextMeshProUGUI takeDownItemTextE;
    public TextMeshProUGUI takeDownItemTextA; // replace with proper names 
    public TextMeshProUGUI takeDownItemTextT;
    public TextMeshProUGUI gemTextE;
    public TextMeshProUGUI gemTextA;
    public TextMeshProUGUI gemTextT;

    public bool hasE;
    public bool hasA;
    public bool hasT;
    public bool hasGemE;
    public bool hasGemA;
    public bool hasGemT;
    public bool hasHelm;

    public int takeDownItemCounterE = 0;
    public int takeDownItemCounterA = 0;
    public int takeDownItemCounterT = 0;
    public int gemCounterE = 0;
    public int gemCounterA = 0;
    public int gemCounterT = 0;
    public int helmcounter = 0;

   // UI ui;
    public bool inventory;

    //public int amount;
    public PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        hasE = false;
        spriteApple.SetActive(false);

        hasA = false;
        spriteSkull.SetActive(false);

        hasT = false;
        spriteFireFlower.SetActive(false);

        hasGemE = false;
        spriteGemE.SetActive(false);

        hasGemA = false;
        spriteGemA.SetActive(false);

        hasGemT = false;
        spriteGemT.SetActive(false);

        hasHelm = false;
        spriteHelm.SetActive(false);

        takeDowntext.SetActive(false);

        invisText.SetActive(false);

        dodgeText.SetActive(false);

        helmUseText.SetActive(false);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //apple
        spriteApple.SetActive(hasE);

        //skull
        spriteSkull.SetActive(hasA);

        //fireFlower
        spriteFireFlower.SetActive(hasT);

        //Gem1
        spriteGemE.SetActive(hasGemE);

        //Gem2
        spriteGemA.SetActive(hasGemA);

        //Gem3
        spriteGemT.SetActive(hasGemT);

        //helm
        spriteHelm.SetActive(hasHelm);
    }

    

    public void ShowAmount(TextMeshProUGUI textChange, int amount, ref bool hasItemName)
    {
        Debug.Log(textChange.text);
        Debug.Log(amount);
        Debug.Log(hasItemName); 
        if (amount < 1)
        {
            textChange.gameObject.SetActive(false);
            hasItemName = false;
            Debug.Log("Apple disappeared, variable is now " + hasItemName);
        }
        else
        {
            textChange.gameObject.SetActive(true);
            textChange.text = "" + amount;
            hasItemName = true;
        }

    }
}
