using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100f;
    [SerializeField]
    private HealthBar healthBar;
    [SerializeField]
    private bool invunarable = false;
    [SerializeField]
    private int[] damageResistances;
    [SerializeField]
    private int[] damageImmunities;

    public bool respawn = false;

    private float currentHealth;
    private ConditionsHandler conditionsHandler;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        conditionsHandler = gameObject.AddComponent<ConditionsHandler>();

        if (damageResistances == null) damageResistances = new int[0];
        if (damageImmunities == null) damageImmunities = new int[0];
        
        
        HealthEventSystem.current.onDamageTaken += TakeDamage;
        HealthEventSystem.current.onDamageIgnoreInvunarableTaken += TakeDamageIgnoreInvunarable;
        HealthEventSystem.current.onChangeInvunerability += SetInvunerability;
        HealthEventSystem.current.onConditionHit += SetCondition;
    }

    public void Damage(float damage, int damageType)
    {
        if (!invunarable)
        {
            DamageIgnoreInvunarable(damage, damageType);
        }
    }

    public void DamageIgnoreInvunarable(float damage, int damageType)
    {
        currentHealth -= CheckDamageTypes(damage, damageType);

        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            if (!respawn)
                Destroy(gameObject);
            else
            {
                currentHealth = maxHealth;
                healthBar.SetHealth(maxHealth);
            }
        }
    }

    private float CheckDamageTypes(float damage, int damageType)
    {
        float dmg = damage;

        foreach (int type in damageImmunities)
        {
            if (type == damageType)
            {
                dmg = 0;
                break;
            }
        }
        if (dmg != 0)
        {
            foreach (int type in damageResistances)
            {
                if (type == damageType)
                {
                    dmg = dmg / 2f;
                    break;
                }
            }
        }

        return dmg;
    }

    private bool IntegerOnArray()
    {

    }

    public void TakeDamage(string name, float damage, int damageType)
    {
        if (gameObject.name == name)
        {
            Damage(damage, damageType);
        }
    }
    public void TakeDamageIgnoreInvunarable(string name, float damage, int damageType)
    {
        if (gameObject.name == name)
        {
            DamageIgnoreInvunarable(damage, damageType);
        }
    }
    public void SetInvunerability(string name, bool state)
    {
        if (gameObject.name == name)
        {
            invunarable = state;
        }
    }
    public void SetCondition(string name, Condition condition)
    {
        if (gameObject.name == name)
        {
            if (invunarable)
            {
                return;
            }
            conditionsHandler.AddCondition(condition);
        }
    }

    private void OnDestroy()
    {
        HealthEventSystem.current.onDamageTaken -= TakeDamage;
        HealthEventSystem.current.onDamageIgnoreInvunarableTaken -= TakeDamageIgnoreInvunarable;
        HealthEventSystem.current.onChangeInvunerability -= SetInvunerability;
        HealthEventSystem.current.onConditionHit -= SetCondition;
    }
}
