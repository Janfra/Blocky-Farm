using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<IDamagable>(out IDamagable health))
        {
            if (health.IsDamagableBy(IDamagable.DamageType.Melee))
            {
                health.Damaged(1f);
            }
        }
    }
}
