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

    public event Action<string, float> onDamageTaken;
    public void TakeDamage(string name, float damage)
    {
        if (onDamageTaken != null)
        {
            onDamageTaken(name, damage);
        }
    }

    public event Action<string, float> onDamageIgnoreShieldsTaken;
    public void TakeDamageIgnoreShields(string name, float damage)
    {
        if (onDamageIgnoreShieldsTaken != null)
        {
            onDamageIgnoreShieldsTaken(name, damage);
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
