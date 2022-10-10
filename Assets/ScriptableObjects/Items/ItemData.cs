using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemData", fileName = "NewItem")]
public class ItemData : ScriptableObject
{
    public string displayName;
    public Sprite icon;
    public ICollectable prefab;
}
