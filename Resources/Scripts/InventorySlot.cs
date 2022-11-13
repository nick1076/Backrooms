using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{

    public Image itemDisplay;
    public Sprite emptySlotIcon;
    [SerializeField] private Item currentItem;

    public void Setup(Item item)
    {
        currentItem = new Item
        {
            item = item.item,
            data = item.data
        };

        itemDisplay.sprite = currentItem.item.uiSprite;
    }

    public Item RetrieveSlotItem()
    {
        return currentItem;
    }
}
