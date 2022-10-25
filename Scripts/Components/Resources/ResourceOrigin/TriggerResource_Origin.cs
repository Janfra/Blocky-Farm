using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerResource_Origin : ResourceOrigin
{
    [SerializeField] private float slowModifier = 1f;

    private void Start()
    {
        OnStart();
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Movement>(out Movement movement))
        {
            Debug.Log($"Bush entered by: {other.gameObject.name}");
            movement.AddSpeedModifier(slowModifier);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<Movement>(out Movement movement))
        {
            movement.AddSpeedModifier(-slowModifier);
        }
    }
}
