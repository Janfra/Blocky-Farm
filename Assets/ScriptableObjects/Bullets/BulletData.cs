using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BulletData", fileName = "NewBullet")]
public class BulletData : ScriptableObject
{
    public float Damage;
    public int MaxAmmo;
    public float ReloadTime;
    public float TimePerShot;
    public float Velocity;
    public float Size;
    public ObjectPooler.poolObjName PoolIndex;
}
