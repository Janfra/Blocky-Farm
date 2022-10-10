using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    /// <summary>
    /// UI/Canvas of the inventory to change visibility
    /// </summary>
    [SerializeField] private GameObject inventoryUI;

    /// <summary>
    /// Boolean to check if the inventory is currently open
    /// </summary>
    private bool onOpen => inventoryUI.activeSelf;

    /// <summary>
    /// Prefab of an empty inventory slot for slot generation.
    /// </summary>
    [SerializeField] private ItemSlot slotPrefab;

    private void Start()
    {
        InventorySystem.OnItemCollected += CreateItem;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeVisibility();
        }
    }

    /// <summary>
    /// Debugging for item creation
    /// </summary>
    /// <param name="data"></param>
    private void CreateItem(ItemData data)
    {
        Debug.Log($"Created new item slot! Item name: {data.DisplayName}");
    }

    /// <summary>
    /// Change UI visibility
    /// </summary>
    public void ChangeVisibility()
    {
        inventoryUI.SetActive(!onOpen);
    }
}
