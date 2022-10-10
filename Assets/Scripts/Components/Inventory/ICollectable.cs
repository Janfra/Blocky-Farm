public interface ICollectable
{
    /// <summary>
    /// Collection handling of object
    /// </summary>
    /// <param name="inventory">Inventory picking up the object</param>
    void Collected(InventorySystem inventory);
    // Default: gameObject.SetActive(!inventory.AddItem(itemData)); 
}
