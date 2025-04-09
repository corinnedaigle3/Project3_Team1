using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
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

    public GameObject takeDowntext;

    public GameObject blankHelm;
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
            blankApple.SetActive(true);

            hasA = false;
            spriteSkull.SetActive(false);
            blankSkull.SetActive(true);

            hasT = false;
            spriteFireFlower.SetActive(false);
            blankFireFlower.SetActive(true);

            hasGemE = false;
            spriteGemE.SetActive(false);
            blankGemE.SetActive(true);

            hasGemA = false;
            spriteGemA.SetActive(false);
            blankGemA.SetActive(true);

            hasGemT = false;
            spriteGemT.SetActive(false);
            blankGemT.SetActive(true);

            hasHelm = false;
            spriteHelm.SetActive(false);
            blankHelm.SetActive(true);

            takeDowntext.SetActive(false);
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
    void Update()
    {
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
