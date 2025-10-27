using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Inventory : MonoBehaviour
{
    // Reference to the player's Inventory object
    private Inventory inventory;

    // Container that holds all the item slots
    private Transform itemSlotContainer;

    // Template used to create new item slots
    private Transform itemSlotTemplate;

    // Called when the script instance is being loaded
    private void Awake()
    {
        // Try to find the itemSlotContainer in the hierarchy
        itemSlotContainer = transform.Find("itemSlotContainer");
        if (itemSlotContainer == null)
        {
            // Error message if container is not found
            Debug.LogError("itemSlotContainer not found!");
            return;
        }

        // Try to find the itemSlotTemplate inside the container
        itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");
        if (itemSlotTemplate == null)
        {
            // Error message if template is missing
            Debug.LogError("itemSlotTemplate not found!");
            return;
        }
    }

    // Method to assign the inventory reference and subscribe to its events
    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;

        // Subscribe to inventory change event
        inventory.OnItemListChanged += Inventory_OnItemListChanged;

        // Initial refresh of inventory display
        RefreshInventoryItems();
    }

    // Event callback triggered whenever the inventory list changes
    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        // Update the inventory UI when items change
        RefreshInventoryItems();
    }

    // Refreshes the UI display of all inventory items
    private void RefreshInventoryItems()
    {
        // Re-find the container and template (in case the object reloaded or changed)
        itemSlotContainer = transform.Find("itemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");

        // Safety check to prevent null reference errors
        if (itemSlotContainer == null)
        {
            Debug.LogError("itemSlotContainer is null!");
            return;
        }

        // Clear old item slots (keep the template itself)
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue; // Skip the template
            Destroy(child.gameObject); // Remove old slot
        }

        // Coordinates for placing slots in a grid
        float x = -1.5f;
        float y = 0;
        float itemSlotCellSize = 120f; // Distance between slots

        // Loop through each item in the inventory list
        foreach (Item item in inventory.GetItemList())
        {
            // Create a new slot from the template
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true); // Make it visible

            // Add a click listener to use the item when the slot is clicked
            itemSlotRectTransform.GetComponent<Button>().onClick.AddListener(() =>
            {
                inventory.UseItem(item);
                Debug.Log("ItemUsed");
            });

            // Set the position of the slot in the grid
            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);

            // Update the item image
            Image image = itemSlotRectTransform.Find("image").GetComponent<Image>();
            image.sprite = item.GetSprite();

            // Update the text showing the item amount
            TextMeshProUGUI uiText = itemSlotRectTransform.Find("text").GetComponent<TextMeshProUGUI>();
            if (item.amount > 1)
            {
                // Display item amount if more than one
                uiText.SetText(item.amount.ToString());
            }
            else
            {
                // Otherwise clear text
                uiText.SetText("");
            }

            // Move to the next slot position
            x++;
            if (x > 8) // Wrap to next row after 9 items
            {
                x = 0;
                y++;
            }
        }
    }
}
