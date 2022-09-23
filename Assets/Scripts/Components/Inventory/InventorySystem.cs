using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    private Dictionary<ItemData, InventoryItem> itemDictionary;
    public List<InventoryItem> inventory { get; private set; }

    [Serializable]
    public class InventoryItem
    {
        public ItemData data { get; private set; }
        public int stackSize { get; private set; }

        public InventoryItem(ItemData newItem)
        {
            data = newItem;
            AddToStack();
        }

        public void AddToStack()
        {
            stackSize++;
        }

        public void RemoveFromStack()
        {
            stackSize--;
        }
    }

    private void Awake()
    {
        inventory = new List<InventoryItem>();
        itemDictionary = new Dictionary<ItemData, InventoryItem>();
    }

    public void AddItem(ItemData itemData)
    {
        if (itemDictionary.TryGetValue(itemData, out InventoryItem value))
        {
            value.AddToStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(itemData);
            inventory.Add(newItem);
            itemDictionary.Add(itemData, newItem);
        }
    }

    public void RemoveItem(ItemData itemData)
    {
        if (itemDictionary.TryGetValue(itemData, out InventoryItem value))
        {
            value.RemoveFromStack();
            if(value.stackSize == 0)
            {
                inventory.Remove(value);
                itemDictionary.Remove(itemData);
            }
        } 
    }
}
