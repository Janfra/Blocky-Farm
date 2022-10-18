using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    /// <summary>
    /// Bullet information
    /// </summary>
    [SerializeField] private BulletData data;

    /// <summary>
    /// Bullet rigidbody
    /// </summary>
    [SerializeField] private Rigidbody rb;

    /// <summary>
    /// Sets the layer to be ignored by bullets.
    /// </summary>
    [SerializeField] private int layerIndexToIgnore;

    void Start()
    {
        Physics.IgnoreLayerCollision(layerIndexToIgnore, layerIndexToIgnore);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{other.gameObject.name} has been hit by a bullet!");
        if (other.TryGetComponent<IDamagable>(out IDamagable objective))
        {
            if (objective.IsDamagableBy(IDamagable.DamageType.Bullet))
            {
                objective.Damaged(data.Damage);
            }
        }
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Sets the velocity of the bullet rigidbody and its direction
    /// </summary>
    /// <param name="velocity">New velocity</param>
    /// <param name="direction">Bullet direction when fired</param>
    public void SetVelocity(float velocity, Vector3 direction)
    {
        rb.velocity = Vector3.zero;
        rb.AddRelativeForce(direction * velocity, ForceMode.Impulse);
    }
}
