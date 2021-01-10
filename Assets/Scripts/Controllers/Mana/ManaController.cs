using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaController : EntityResource
{
    private new void Awake()
    {
        base.Awake();
    }

    private new void Start()
    {
        base.Start();

        ManaEventSystem.current.onManaUsed += ReduceMana;
    }

    private void OnDestroy()
    {
        ManaEventSystem.current.onManaUsed -= ReduceMana;
    }

    public void ReduceMana(float reduction)
    {
        currentValue = currentValue - reduction;
    }
}
