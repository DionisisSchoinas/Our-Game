using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaController : EntityResource
{
    public new float currentValue
    {
        get
        {
            return base.currentValue;
        }
        set
        {
            base.currentValue = value;
            ManaEventSystem.current.UpdateMana(base.currentValue);
        }
    }

    private new void Awake()
    {
        base.Awake();
    }

    private new void Start()
    {
        base.Start();

        ManaEventSystem.current.UpdateMana(currentValue);

        ManaEventSystem.current.onManaUsed += ReduceMana;
    }

    private void OnDestroy()
    {
        ManaEventSystem.current.onManaUsed -= ReduceMana;
    }

    public void ReduceMana(float reduction)
    {
        currentValue = currentValue - reduction;

        ManaEventSystem.current.UpdateMana(currentValue);
    }

    // Override Regen since we want to use the new getter for currentValue
    protected override IEnumerator Regen()
    {
        finsihedRegen = false;
        while (currentValue < maxValue)
        {
            currentValue = currentValue + regenPerSecond / 10f;
            yield return new WaitForSeconds(0.1f);
        }
        finsihedRegen = true;
        yield return null;
    }
}
