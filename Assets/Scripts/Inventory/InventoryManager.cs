using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public List<GameObject> inventoryList = new List<GameObject>();

    public void AddItem(GameObject item)
    {
        inventoryList.Add(item);
    }

    public void RemoveItem(GameObject item)
    {
        inventoryList.Remove(item);
    }
}