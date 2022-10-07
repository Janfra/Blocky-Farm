using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemData", fileName = "NewItem")]
public class ItemData : ScriptableObject
{
    public string DisplayName;
    public int StackLimit;
    public Sprite Icon;
    public ObjectPooler.poolObjName PoolIndex;
}
