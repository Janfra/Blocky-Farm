using UnityEngine;

public class Resource : MonoBehaviour, ICollectable
{
    /// <summary>
    /// Resource to be spawn
    /// </summary>
    [SerializeField] private ItemData itemData;

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Entered");
        if(collider.TryGetComponent(out InventorySystem inventory))
        {
            Collected(inventory);
        }
    }

    /// <summary>
    /// Disables game object if the item was added to the inventory.
    /// </summary>
    /// <param name="inventory"></param>
    public void Collected(InventorySystem inventory)
    {
        gameObject.SetActive(!inventory.AddItem(itemData));
    }

    /// <summary>
    /// Gets the index of the pool of this object.
    /// </summary>
    /// <returns>Pool index</returns>
    public ObjectPooler.poolObjName GetPoolName()
    {
        return itemData.PoolIndex;
    }
}
