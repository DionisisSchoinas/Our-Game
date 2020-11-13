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
}
