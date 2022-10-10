using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Stored_LoadingBar))]
public class Health : MonoBehaviour, IDamagable
{
    #region Variables

    /// <summary>
    /// Scriptable Object for HP
    /// </summary>
    [SerializeField] protected HealthData data;
    
    /// <summary>
    /// UI loading bar
    /// </summary>
    [SerializeField] protected Stored_LoadingBar healthBar;

    /// <summary>
    ///  Damage type that affects this object
    /// </summary>
    [SerializeField] protected IDamagable.DamageType damagableBy;

    /// <summary>
    /// Current HP
    /// </summary>
    protected float health;

    #endregion

    private void Start()
    {
        healthBar.SetSliderMax(data.MaxHealth);
        health = data.MaxHealth;
    }

    #region Damage Handling

    /// <summary>
    /// Deal damage, show it on the UI and disable object if dead.
    /// </summary>
    /// <param name="damageTaken">The damage dealt to the object</param>
    virtual public void Damaged(float damageTaken)
    {
        health -= damageTaken;
        healthBar.SetLoadTarget(health, health + damageTaken);
        gameObject.SetActive(IsAlive());
    }

    /// <summary>
    /// Check if the object still has HP left
    /// </summary>
    /// <returns>Is the object alive</returns>
    protected bool IsAlive()
    {
        return health > 0f;
    }

    /// <summary>
    /// Checks if the object is damagable
    /// </summary>
    /// <param name="damageType">The type of damage being applied</param>
    /// <returns>If it is possible to damage the object</returns>
    public bool IsDamagableBy(IDamagable.DamageType damageType)
    {
        return damagableBy == damageType || damagableBy == IDamagable.DamageType.Every;
    }

    #endregion
}
