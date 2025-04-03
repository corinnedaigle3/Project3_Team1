using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject blankApple;
    public GameObject spriteApple;

    public GameObject blankSkull;
    public GameObject spriteSkull;

    public GameObject blankFireFlower;
    public GameObject spriteFireFlower;

    public GameObject blankGemT;
    public GameObject spriteGemT;

    public GameObject blankGemA;
    public GameObject spriteGemA;

    public GameObject blankGemE;
    public GameObject spriteGemE;

    public GameObject blankHelm;
    public GameObject spriteHelm;
    public TextMeshProUGUI helmText;
    public TextMeshProUGUI TakeDownItemEText;
    public TextMeshProUGUI TakeDownItemAText; // replace with proper names 
    public TextMeshProUGUI TakeDownItemTText;
    public TextMeshProUGUI gemEText;
    public TextMeshProUGUI gemAText;
    public TextMeshProUGUI gemTText;

    public bool hasApple;
    public bool hasSkull;
    public bool hasFireFlower;
    public bool hasGemE;
    public bool hasGemA;
    public bool hasGemT;
    public bool hasHelm;

    public int TakeDownItemEcounter = 0;
    public int TakeDownItemAcounter = 0;
    public int TakeDownItemTcounter = 0;
    public int GemEcounter = 0;
    public int GemAcounter = 0;
    public int GemTcounter = 0;
    public int helmcounter = 0;

    UI ui;
    public bool inventory;

    //public int amount;
    public PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        if (ui.inventory == true)
        {
            hasApple = false;
            spriteApple.SetActive(false);
            blankApple.SetActive(true);

            hasSkull = false;
            spriteSkull.SetActive(false);
            blankSkull.SetActive(true);

            hasFireFlower = false;
            spriteFireFlower.SetActive(false);
            blankFireFlower.SetActive(true);

            hasGemE = false;
            spriteGemT.SetActive(false);
            blankGemT.SetActive(true);

            hasGemA = false;
            spriteGemA.SetActive(false);
            blankGemA.SetActive(true);

            hasGemT = false;
            spriteGemE.SetActive(false);
            blankGemE.SetActive(true);

            hasHelm = false;
            spriteHelm.SetActive(false);
            blankHelm.SetActive(true);

        }

    }

    // Update is called once per frame
    void Update()
    {
        //apple
        if (hasApple == true)
        {
            spriteApple.SetActive(true);
        }
        else if (hasApple == false)
        {
            spriteApple.SetActive(false);
        }

        //skull
        if (hasSkull == true)
        {
            spriteSkull.SetActive(true);
        }
        else if (hasSkull == false)
        {
            spriteSkull.SetActive(false);
        }

        //fireFlower
        if (hasFireFlower == true)
        {
            spriteFireFlower.SetActive(true);
        }
        else if (hasFireFlower == false)
        {
            spriteFireFlower.SetActive(false);
        }

        //Gem1
        if (hasGemE == true)
        {
            spriteGemT.SetActive(true);
        }
        else if (hasGemE == false)
        {
            spriteGemT.SetActive(false);
        }

        //Gem2
        if (hasGemA == true)
        {
            spriteGemA.SetActive(true);
        }
        else if (hasGemA == false)
        {
            spriteGemA.SetActive(false);
        }

        //Gem3
        if (hasGemT == true)
        {
            spriteGemE.SetActive(true);
        }
        else if (hasGemT == false)
        {
            spriteGemE.SetActive(false);
        }

        //helm
        if (hasHelm == true)
        {
            spriteHelm.SetActive(true);
        }
        else if (hasHelm == false)
        {
            spriteHelm.SetActive(false);
        }
    }

    public void ShowAmount(TextMeshProUGUI textChange, int amount)
    {
        if (amount == 0)
        {
            textChange.gameObject.SetActive(false);
        }
        else if (amount == 1)
        {            
            textChange.text = "" + amount;
            textChange.gameObject.SetActive(true);
        }
        else if (amount == 2)
        {        
            textChange.text = "" + amount;
            textChange.gameObject.SetActive(true);
        }
        else if (amount == 3)
        {
            textChange.text = "" + amount;
            textChange.gameObject.SetActive(true);
        }
       
    }
}
