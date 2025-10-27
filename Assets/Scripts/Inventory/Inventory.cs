using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles storage and management of items the player owns.
// Supports stackable and non-stackable items and triggers events when the inventory changes.
public class Inventory
{
    // Event that fires whenever the item list changes (e.g., add/remove)
    // Other scripts can subscribe to this to update UI or respond to inventory changes.
    public event EventHandler OnItemListChanged;

    // The actual list of items currently held in the inventory
    private List<Item> itemList;

    // A reference to an external action that defines how to use an item.
    // This allows the Inventory class to remain generic — it doesn’t know what “using” an item means.
    private Action<Item> useItemAction;

    // Constructor: initializes the inventory and stores the "use item" callback.
    public Inventory(Action<Item> useItemAction)
    {
        this.useItemAction = useItemAction;
        itemList = new List<Item>();
    }

    // Adds an item to the inventory
    public void AddItem(Item item)
    {
        // If the item type can stack (like potions or arrows)
        if (item.IsStackable())
        {
            bool itemAlreadyInInventory = false;

            // Check if the same type of item already exists
            foreach (Item inventroyItem in itemList)
            {
                if (inventroyItem.itemType == item.itemType)
                {
                    // If found, increase its amount instead of adding a new entry
                    inventroyItem.amount += item.amount;
                    itemAlreadyInInventory = true;
                }
            }

            // If it’s a new stackable item type, add it to the list
            if (!itemAlreadyInInventory)
            {
                itemList.Add(item);
            }
        }
        else
        {
            // Non-stackable items (like weapons or equipment) are always added as separate entries
            itemList.Add(item);
        }

        // Notify listeners (like UI) that the inventory changed
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    // Removes an item from the inventory
    public void RemoveItem(Item item)
    {
        if (item.IsStackable())
        {
            Item itemInInventory = null;

            // Find the stack of this item type
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    // Subtract the specified amount
                    inventoryItem.amount -= item.amount;
                    itemInInventory = inventoryItem;
                }
            }

            // If the stack’s amount drops to 0 or below, remove it entirely
            if (itemInInventory != null && itemInInventory.amount <= 0)
            {
                itemList.Remove(itemInInventory);
            }
        }
        else
        {
            // Non-stackable items are removed directly
            itemList.Remove(item);
        }

        // Notify listeners that the inventory changed
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    // Uses an item — the actual effect is handled externally via the provided Action<Item>
    public void UseItem(Item item)
    {
        useItemAction(item);
    }

    // Returns the current list of items (for display or saving)
    public List<Item> GetItemList()
    {
        return itemList;
    }
}
