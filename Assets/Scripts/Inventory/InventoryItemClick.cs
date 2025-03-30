using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemClick : MonoBehaviour
{
    // Reference to the inventory script or list
    public InventoryManager inventoryManager; // Or your inventory list

    void OnMouseDown()
    {
        // Destroy the item
        Destroy(gameObject);

        // Remove from inventory list
        if (inventoryManager != null)
        {
            inventoryManager.RemoveItem(gameObject);
        }
    }
}
