using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEventSystem : MonoBehaviour
{
    public static HealthEventSystem current;

    private void Awake()
    {
        current = this;
    }

    public event Action<string, float, int> onDamageTaken;
    public void TakeDamage(string name, float damage, int damageType)
    {
        if (onDamageTaken != null)
        {
            onDamageTaken(name, damage, damageType);
        }
    }

    public event Action<string, float, int> onDamageIgnoreInvunarableTaken;
    public void TakeDamageIgnoreShields(string name, float damage, int damageType)
    {
        if (onDamageIgnoreInvunarableTaken != null)
        {
            onDamageIgnoreInvunarableTaken(name, damage, damageType);
        }
    }

    public event Action<string, bool> onChangeInvunerability;
    public void SetInvunerable(string name, bool state)
    {
        if (onChangeInvunerability != null)
        {
            onChangeInvunerability(name, state);
        }
    }

    public event Action<string, Condition> onConditionHit;
    public void SetCondition(string name, Condition condition)
    {
        if (onConditionHit != null)
        {
            onConditionHit(name, condition);
        }
    }
}
