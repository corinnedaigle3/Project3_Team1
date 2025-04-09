using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType
    {
        TakeDownItemE,
        TakeDownItemA,
        TakeDownItemT,
        Gem1,
        Gem2,
        Gem3,
        Helm
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.TakeDownItemE: return ItemAssets.Instance.appleSprite;
            case ItemType.TakeDownItemA: return ItemAssets.Instance.skullSprite;
            case ItemType.TakeDownItemT: return ItemAssets.Instance.fireFlowerSprite;
            case ItemType.Gem1: return ItemAssets.Instance.gemSprite1;
            case ItemType.Gem2: return ItemAssets.Instance.gemSprite2;
            case ItemType.Gem3: return ItemAssets.Instance.gemSprite3;
            case ItemType.Helm: return ItemAssets.Instance.helmSprite;
        }
    }

    public bool IsStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.Helm:
                //return true;
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
