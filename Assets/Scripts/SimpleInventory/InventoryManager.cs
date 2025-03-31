using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        hasApple = false;
        spriteApple.SetActive(false);

        spriteSkull.SetActive(false);
        spriteFireFlower.SetActive(false);
        spriteGem1.SetActive(false);
        spriteGem2.SetActive(false);
        spriteGem3.SetActive(false);
        spriteHelm.SetActive(false);
        //helmTextBlank.text = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasApple == true)
        {
            spriteApple.SetActive(true);
        }
        else if (hasApple == false)
        {
            spriteApple.SetActive(false);
        }
    }
}
