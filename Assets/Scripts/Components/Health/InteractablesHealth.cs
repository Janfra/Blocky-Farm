using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablesHealth : Health
{
    /// <summary>
    /// UI handler for interactions, added to cancel interactions on damage
    /// </summary>
    [SerializeField] private Interact_LoadingBar interactionLoader;

    private void Awake()
    {
        Init();
        if(interactionLoader == null)
        {
            Debug.LogError($"{gameObject.name} has no Interct Bar");
            interactionLoader = GetComponent<Interact_LoadingBar>();
        }
    }

    // Overrwritten to cancel interactions on damage
    public override void Damaged(float damageTaken)
    {
        base.Damaged(damageTaken);
        interactionLoader.isCanceled = true;
    }
}
