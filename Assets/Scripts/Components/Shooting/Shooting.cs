using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shooting : MonoBehaviour
{
    /// <summary>
    /// Bullet information
    /// </summary>
    [SerializeField] protected BulletData bullet;

    /// <summary>
    /// Shooting handling
    /// </summary>
    abstract protected void Shot();

    /// <summary>
    /// Reloading handling
    /// </summary>
    abstract protected void Reload();

    /// <summary>
    /// Spawns bullet at the given transform then sets is velocity and direction.
    /// </summary>
    /// <param name="spawnTransform"></param>
    virtual protected void SpawnBulletAt(Transform spawnTransform)
    {
        Bullet currentBullet = ObjectPooler.Instance.SpawnFromPool(bullet.PoolIndex, spawnTransform.position, Quaternion.identity).GetComponent<Bullet>();
        currentBullet.SetVelocity(bullet.Velocity, transform.forward);
    }
}
