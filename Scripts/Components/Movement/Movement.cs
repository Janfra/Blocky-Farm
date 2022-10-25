using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    [SerializeField] protected MoveStats baseSpeed;
    protected float speedModifier;

    private void Awake()
    {
        Init();
    }

    protected void Init()
    {
        if(baseSpeed == null)
        {
            Debug.LogError($"{gameObject.name} has no Movement base stats");
            baseSpeed = new MoveStats(3f);
        }
    }

    virtual protected float GetCurrentSpeed()
    {
        float newSpeed = baseSpeed.speed + speedModifier;
        if (newSpeed < 0)
        {
            return 0;
        }
        else
        {
            return newSpeed;
        }
    }

    public void AddSpeedModifier(float newModifier)
    {
        speedModifier += newModifier;
    }

    abstract protected void Move();
}