using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    [Header("Inventory Settings")]
    public int itemCap = 2;
    public bool selectOnAdd = false;

    public bool hasDisplay;
    public InventoryDisplay invenDisplay;

    [Header("For Viewing/Debugging")]
    [SerializeField] private List<Item> currentItems;
    [SerializeField] private Item selected;
    [SerializeField] private int selectedSlotNumber;

    private void Start()
    {
        if (hasDisplay)
        {
            invenDisplay.DisplayInventory(this);
        }
    }

    public bool AddItem(Item item)
    {
        foreach (Item itemInfo in currentItems)
        {
            if (itemInfo.item == item.item)
            {
                return false;
            }
        }

        if (currentItems.Count >= itemCap)
        {
            return false;
        }
        else
        {
            currentItems.Add(item);

            if (selectOnAdd)
            {
                for (int i = 0; i < currentItems.Count; i++)
                {
                    if (currentItems[i] == item)
                    {
                        SelectItem(i);
                        if (hasDisplay)
                        {
                            invenDisplay.DisplayInventory(this);
                        }
                        return true;
                    }
                }
            }

            if (hasDisplay)
            {
                invenDisplay.DisplayInventory(this);
            }
            return true;
        }
    }

    public bool RemoveItem(ItemData item)
    {
        foreach(Item itemInfo in currentItems)
        {
            if (itemInfo.item == item)
            {
                currentItems.Remove(itemInfo);
                if (selected.item == itemInfo.item)
                {
                    selected.item = null;
                    selected.data = null;
                }
                if (hasDisplay)
                {
                    invenDisplay.DisplayInventory(this);
                }
                return true;
            }
        }
        return false;
    }

    public List<Item> RetrieveCurrentInventory()
    {
        return currentItems;
    }

    public Item RetrieveCurrentlySelected()
    {
        return selected;
    }

    public void ClearInventory()
    {
        currentItems.Clear();
        selected.item = null;
        currentItems = new List<Item>();

        if (hasDisplay)
        {
            invenDisplay.DisplayInventory(this);
        }
    }

    public bool SelectItem(int which)
    {
        if (which == -1)
        {
            selected.item = null;
            selectedSlotNumber = 9;
            return true;
        }

        selectedSlotNumber = which;
        if (which + 1 > itemCap)
        {
            if (hasDisplay)
            {
                invenDisplay.DisplayInventory(this);
            }

            return false;
        }

        if (which > currentItems.Count - 1)
        {
            selected.item = null;

            if (hasDisplay)
            {
                invenDisplay.DisplayInventory(this);
            }

            return false;
        }
        else
        {
            if (selected.item == currentItems[which].item && selected.data == currentItems[which].data)
            {
                selected.item = null;
            }
            else
            {
                selected = new Item
                {
                    item = currentItems[which].item,
                    data = currentItems[which].data,
                };
            }

            if (hasDisplay)
            {
                invenDisplay.DisplayInventory(this);
            }

            return true;
        }
    }

    public int RetrieveSelectedSlotNumber()
    {
        return selectedSlotNumber;
    }
}
