using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaController : EntityResource
{
    private new void Awake()
    {
        base.Awake();
    }

    private void OnDestroy()
    {
    }

    public void ReduceMana(float reduction)
    {
        currentValue -= reduction;
    }
}
