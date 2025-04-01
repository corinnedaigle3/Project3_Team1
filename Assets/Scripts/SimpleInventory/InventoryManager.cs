using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour, IPointerClickHandler
{
    public GameObject blankApple;
    public GameObject spriteApple;

    public GameObject blankSkull;
    public GameObject spriteSkull;

    public GameObject blankFireFlower;
    public GameObject spriteFireFlower;

    public GameObject blankGem1;
    public GameObject spriteGem1;

    public GameObject blankGem2;
    public GameObject spriteGem2;

    public GameObject blankGem3;
    public GameObject spriteGem3;

    public GameObject blankHelm;
    public GameObject spriteHelm;
    public TextMeshProUGUI helmTextBlank;
    public TextMeshProUGUI helmText2;
    public TextMeshProUGUI helmText3;

    public bool hasApple;
    public bool hasSkull;
    public bool hasFireFlower;
    public bool hasGem1;
    public bool hasGem2;
    public bool hasGem3;
    public bool hasHelm;

    // Start is called before the first frame update
    void Start()
    {
        hasApple = false;
        spriteApple.SetActive(false);

        hasSkull = false;
        spriteSkull.SetActive(false);

        hasFireFlower = false;
        spriteFireFlower.SetActive(false);

        hasGem1 = false;
        spriteGem1.SetActive(false);

        hasGem2 = false;
        spriteGem2.SetActive(false);

        hasGem3 = false;
        spriteGem3.SetActive(false);

        hasHelm = false;
        spriteHelm.SetActive(false);
        //helmTextBlank.text = false;
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
        if (hasGem1 == true)
        {
            spriteGem1.SetActive(true);
        }
        else if (hasGem1 == false)
        {
            spriteGem1.SetActive(false);
        }

        //Gem2
        if (hasGem2 == true)
        {
            spriteGem2.SetActive(true);
        }
        else if (hasGem2 == false)
        {
            spriteGem2.SetActive(false);
        }

        //Gem3
        if (hasGem3 == true)
        {
            spriteGem3.SetActive(true);
        }
        else if (hasGem3 == false)
        {
            spriteGem3.SetActive(false);
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

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Left Click");
        if (eventData.button == PointerEventData.InputButton.Left && hasApple == true)
        {
            Destroy(this);
        }

        if (eventData.button == PointerEventData.InputButton.Left && hasSkull == true)
        {
            Destroy(this);
        }

        if (eventData.button == PointerEventData.InputButton.Left && hasFireFlower == true)
        {
            Destroy(this);
        }
    }
}
