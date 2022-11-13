using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour
{
    public Transform slotOrigin;
    public GameObject defaultSlot;

    private List<InventorySlot> inventorySlots = new List<InventorySlot>();

    public bool on;

    public void DisplayInventory(Inventory attachedInventory)
    {
        if (on == false)
        {
            return;
        }
        if (inventorySlots.Count > 0)
        {
            foreach (InventorySlot slot in inventorySlots)
            {
                Destroy(slot.gameObject);
            }

            inventorySlots.Clear();
        }

        List<Item> newItems = new List<Item>();
        newItems = attachedInventory.RetrieveCurrentInventory();

        for (int i = 0; i < attachedInventory.itemCap; i++)
        {
            InventorySlot createdSlot = Instantiate(defaultSlot, slotOrigin.transform).GetComponent<InventorySlot>();
            inventorySlots.Add(createdSlot);

            if (i == attachedInventory.RetrieveSelectedSlotNumber())
            {
                createdSlot.GetComponent<UnityEngine.UI.Image>().color = new Color(0.9978545f, 1, .4f, 0.6509804f);
            }
            else
            {
                createdSlot.GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0.6509804f);
            }

            if (newItems.Count - 1 >= i)
            {
                inventorySlots[i].Setup(newItems[i]);
            }
        }
    }
}
