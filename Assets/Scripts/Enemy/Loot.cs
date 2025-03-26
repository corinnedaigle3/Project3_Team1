using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Loot : MonoBehaviour
{
    [SerializeField] private GameObject fury1Gem;

    private Item item;

    public void Initialize(Item item)
    {
        this.item = item;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Destroy(gameObject);
        }
    }
}
