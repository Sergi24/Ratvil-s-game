using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Inventory Inventory;

    private void Start()
    {
        Inventory.ItemAdded += InventoryScript_ItemAdded;
    }

    private void InventoryScript_ItemAdded(object sender, InventoryEventArgs e)
    {
        // Get Inventory Panel
        Transform inventoryPanel = transform.Find("InventoryPanel");

        // Loop through Inventory Panel's slots to find an empty slot
        foreach (Transform slot in inventoryPanel)
        {
            // Border > Image
            Image image = slot.GetChild(0).GetChild(0).GetComponent<Image>();

            // If empty slot found
            if (!image.enabled)
            {
                // Assign item to slot
                image.enabled = true;
                image.sprite = e.Item.Image;

                break;
            }
        }
    }
}
