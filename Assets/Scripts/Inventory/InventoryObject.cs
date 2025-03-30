using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject, IPointerClickHandler
{
    public List<InventorySlot> Container = new List<InventorySlot>();
    public bool hasItem;
    Item item;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && hasItem == true)
        {
            Destroy(item);
        }
    }

    public void AddItem(ItemObject _item, int _amount)
    {
        hasItem = false;
        for (int i = 0; i < Container.Count; i++) 
        {
            if (Container[i].item == _item) 
            {
                Container[i].AddAmount(_amount);
                hasItem = true;
                break;
            }
        }
        if(!hasItem)
        {
            Container.Add(new InventorySlot(_item, _amount));
        }
    }
}

[System.Serializable]
public class InventorySlot
{
    public ItemObject item;
    public int amount;
    public InventorySlot(ItemObject _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }

    public void RemoveAmount(int value) 
    { 
        amount -= value;
    }
}