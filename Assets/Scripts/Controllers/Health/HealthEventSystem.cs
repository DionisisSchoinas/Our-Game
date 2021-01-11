﻿using System;
using UnityEngine;

public class HealthEventSystem : MonoBehaviour
{
    public static HealthEventSystem current;

    private void Awake()
    {
        current = this;
    }

    // Deals damage ignoring invunarable
    public event Action<string, float, int> onDamageTaken;
    public void TakeDamage(string name, float damage, int damageType)
    {
        if (onDamageTaken != null)
        {
            onDamageTaken(name, damage, damageType);
        }
    }
    
    // Deals damage
    public void TakeDamageWithoutEvent(GameObject target, float damage, int damageType)
    {
        if (LayerMask.GetMask(LayerMask.LayerToName(target.gameObject.layer)) == BasicLayerMasks.DamageableEntities)
            target.GetComponent<HealthController>().Damage(damage, damageType);
    }
    
    // Deals damage ignoring invunarable
    public event Action<string, float, int> onDamageIgnoreInvunarableTaken;
    public void TakeDamageIgnoreShields(string name, float damage, int damageType)
    {
        if (onDamageIgnoreInvunarableTaken != null)
        {
            onDamageIgnoreInvunarableTaken(name, damage, damageType);
        }
    }
    // Sets the invunarablility state
    public event Action<string, bool> onChangeInvunerability;
    public void SetInvunerable(string name, bool state)
    {
        if (onChangeInvunerability != null)
        {
            onChangeInvunerability(name, state);
        }
    }
    // Applies a condition
    public event Action<string, Condition> onConditionHit;
    public void SetCondition(string name, Condition condition)
    {
        if (onConditionHit != null)
        {
            onConditionHit(name, condition);
        }
    }
    // Applies force
    public event Action<string, Vector3, float> onForceApply;
    public void ApplyForce(string name, Vector3 direction, float magnitude)
    {
        if (onForceApply != null)
        {
            onForceApply(name, direction.normalized, magnitude);
        }
    }
    // Applies resistance
    public event Action<string, SkinnedMeshRenderer, Material, int, float> onResistanceApply;
    public void ApplyResistance(string name, SkinnedMeshRenderer mesh, Material newMaterial, int resistance, float duration)
    {
        if (onResistanceApply != null)
        {
            onResistanceApply(name, mesh, newMaterial, resistance, duration);
        }
    }
}