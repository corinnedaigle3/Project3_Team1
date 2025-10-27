using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represents an individual item that can exist in the player's inventory.
// Each item has a type, an amount, and a way to get its visual representation (sprite).
public class Item
{
    // Enum defines all possible types of items in the game.
    // These can represent consumables, collectibles, or equipment.
    public enum ItemType
    {
        TakeDownItemE,  // Example: Enemy-type E takedown reward item
        TakeDownItemA,  // Example: Enemy-type A takedown reward item
        TakeDownItemT,  // Example: Enemy-type T takedown reward item
        Gem1,            // Collectible gem type 1
        Gem2,            // Collectible gem type 2
        Gem3,            // Collectible gem type 3
        Helm             // Example of an equipment-type item
    }

    // The specific type of this item
    public ItemType itemType;

    // How many of this item the player has
    public int amount;

    // Returns the corresponding sprite for the item type
    // This allows the UI or world objects to display the correct image.
    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.TakeDownItemE: return ItemAssets.Instance.appleSprite;       // Example: enemy E takedown item icon
            case ItemType.TakeDownItemA: return ItemAssets.Instance.skullSprite;       // Example: enemy A takedown item icon
            case ItemType.TakeDownItemT: return ItemAssets.Instance.fireFlowerSprite;  // Example: enemy T takedown item icon
            case ItemType.Gem1: return ItemAssets.Instance.gemSprite1;                 // Gem 1 icon
            case ItemType.Gem2: return ItemAssets.Instance.gemSprite2;                 // Gem 2 icon
            case ItemType.Gem3: return ItemAssets.Instance.gemSprite3;                 // Gem 3 icon
            case ItemType.Helm: return ItemAssets.Instance.helmSprite;                 // Helmet icon
        }
    }

    // Determines if the item can stack in the inventory
    // (i.e., if multiple of this item can share a single slot)
    public bool IsStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.Helm:
            case ItemType.TakeDownItemE:
            case ItemType.TakeDownItemA:
            case ItemType.TakeDownItemT:
            case ItemType.Gem1:
            case ItemType.Gem2:
            case ItemType.Gem3:
                return true;
        }
    }
}
