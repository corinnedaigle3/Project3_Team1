using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Inventory : MonoBehaviour
{
    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;

    private void Awake()
    {
        itemSlotContainer = transform.Find("itemSlotContainer");
        if (itemSlotContainer == null)
        {
            Debug.LogError("itemSlotContainer not found!");
            return;
        }
        itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");
        if (itemSlotTemplate == null)
        {
            Debug.LogError("itemSlotTemplate not found!");
            return;
        }
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;

        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }

    private void RefreshInventoryItems()
    {
        itemSlotContainer = transform.Find("itemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");

        if (itemSlotContainer == null)
        {
            Debug.LogError("itemSlotContainer is null!");
            return;
        }

        foreach (Transform child in itemSlotContainer)
        {
           if (child == itemSlotTemplate) continue;
           Destroy(child.gameObject);
        }

        float x = -1.5f;
        float y = 0;
        float itemSlotCellSize = 120f;

        foreach (Item item in inventory.GetItemList())
        {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);

            itemSlotRectTransform.GetComponent<Button>().onClick.AddListener(() =>
            {
                inventory.UseItem(item);
                Debug.Log("ItemUsed");
            });
           
            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
            Image image = itemSlotRectTransform.Find("image").GetComponent<Image>();
            image.sprite = item.GetSprite();

            TextMeshProUGUI uiText = itemSlotRectTransform.Find("text").GetComponent<TextMeshProUGUI>();
            if (item.amount > 1)
            {
                uiText.SetText(item.amount.ToString());
            }
            else 
            {
                uiText.SetText("");
            }

            x++;
            if(x > 8)
            {
                x = 0;
                y++;
            }
        }
    }
}
