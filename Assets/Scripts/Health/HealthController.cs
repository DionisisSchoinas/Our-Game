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

    public bool respawn = false;

    private float currentHealth;
    private ConditionsHandler conditionsHandler;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        conditionsHandler = gameObject.AddComponent<ConditionsHandler>();
        
        HealthEventSystem.current.onDamageTaken += TakeDamage;
        HealthEventSystem.current.onDamageIgnoreShieldsTaken += TakeDamageIgnoreShields;
        HealthEventSystem.current.onChangeInvunerability += SetInvunerability;
        HealthEventSystem.current.onConditionHit += SetCondition;
    }

    public void Damage(float damage)
    {
        if (!invunarable)
        {
            DamageIgnoreShields(damage);
        }
    }

    public void DamageIgnoreShields(float damage)
    {
        currentHealth -= damage;
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

    public void TakeDamage(string name, float damage)
    {
        if (gameObject.name == name)
        {
            Damage(damage);
        }
    }
    public void TakeDamageIgnoreShields(string name, float damage)
    {
        if (gameObject.name == name)
        {
            DamageIgnoreShields(damage);
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
        HealthEventSystem.current.onDamageTaken -= TakeDamageIgnoreShields;
        HealthEventSystem.current.onChangeInvunerability -= SetInvunerability;
        HealthEventSystem.current.onConditionHit -= SetCondition;
    }
}
