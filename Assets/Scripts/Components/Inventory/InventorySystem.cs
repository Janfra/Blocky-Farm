using System;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    /// <summary>
    /// Event when an item is added to inventory to generate new UI slot. 
    /// </summary>
    static public event Action<ItemData> OnItemCollected;

    // NOTE: new events will probably be required for deleting slots and updating them.

    /// <summary>
    /// Dictionary holding all items collected information, avoids duplication.
    /// </summary>
    private Dictionary<ItemData, InventoryItem> itemDictionary;

    /// <summary>
    /// List of information of each unique item type
    /// </summary>
    public List<InventoryItem> Inventory { get; private set; }

    /// <summary>
    /// Class to define individual stack handling.
    /// </summary>
    [Serializable]
    public class InventoryItem
    {
        /// <summary>
        /// Stores item information
        /// </summary>
        public ItemData Data { get; private set; }

        /// <summary>
        /// Current size of the stack
        /// </summary>
        public int StackSize { get; private set; }

        /// <summary>
        /// Constructor for stack
        /// </summary>
        /// <param name="newItem">Type of item of this stack</param>
        public InventoryItem(ItemData newItem)
        {
            Data = newItem;
            AddToStack();
        }

        /// <summary>
        /// Adds one more item to the item stack if possible.
        /// </summary>
        /// <returns>If the item was succesfully added to the stack.</returns>
        public bool AddToStack()
        {
            if(StackSize >= Data.StackLimit)
                return false;
 
            StackSize++;
            Debug.Log($"{Data.DisplayName} has been added. Stack size: {StackSize}");
            return true;
        }

        /// <summary>
        /// Removes one item from the stack
        /// </summary>
        public void RemoveFromStack()
        {
            StackSize--;
        }
    }

    private void Awake()
    {
        Inventory = new List<InventoryItem>();
        itemDictionary = new Dictionary<ItemData, InventoryItem>();
    }

    /// <summary>
    /// Tries to add an item to the inventory.
    /// </summary>
    /// <param name="itemData">Item to be added</param>
    /// <returns> If the item was succesfully added to the inventory</returns>
    public bool AddItem(ItemData itemData)
    {
        // Check if the item already exists in the dictionary/inventory, if it does, try to add one to the stack.
        if (itemDictionary.TryGetValue(itemData, out InventoryItem value))
        {
            return value.AddToStack();
        }
        else
        {
            // If it doesnt, create it.
            InventoryItem newItem = new InventoryItem(itemData);
            Inventory.Add(newItem);
            itemDictionary.Add(itemData, newItem);
            OnItemCollected?.Invoke(itemData);
            return true;
        }
    }

    /// <summary>
    /// Try to remove one item from the inventory.
    /// </summary>
    /// <param name="itemData">Item to be removed</param>
    public void RemoveItem(ItemData itemData)
    {
        if (itemDictionary.TryGetValue(itemData, out InventoryItem value))
        {
            value.RemoveFromStack();
            if(value.StackSize == 0)
            {
                // If the stack is empty remove it from the inventory collected information.
                Inventory.Remove(value);
                itemDictionary.Remove(itemData);
            }
        } 
    }
}
