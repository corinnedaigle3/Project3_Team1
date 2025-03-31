using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject spriteApple;
    public bool hasApple;

    // Start is called before the first frame update
    void Start()
    {
        hasApple = false;
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
            spriteApple.SetActive(true);
        }
    }
}
