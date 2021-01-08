using System.Collections.Generic;
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
    private ResistanceHandler resistanceHandler;
    private HitStop hitStop;

    //temp
    Rigidbody rb;



    // Start is called before the first frame update
    void Start()
    {
        if (healthBar == null)
            healthBar = gameObject.AddComponent<HealthBar>();
        conditionsHandler = gameObject.AddComponent<ConditionsHandler>();
        resistanceHandler = gameObject.AddComponent<ResistanceHandler>();

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        rb = gameObject.GetComponent<Rigidbody>() as Rigidbody;
        hitStop = FindObjectOfType<HitStop>();
        
        HealthEventSystem.current.onDamageIgnoreInvunarableTaken += TakeDamageIgnoreInvunarable;
        HealthEventSystem.current.onChangeInvunerability += SetInvunerability;
        HealthEventSystem.current.onConditionHit += SetCondition;
        HealthEventSystem.current.onForceApply += ApplyForce;
    }
    private void OnDestroy()
    {
        HealthEventSystem.current.onDamageIgnoreInvunarableTaken -= TakeDamageIgnoreInvunarable;
        HealthEventSystem.current.onChangeInvunerability -= SetInvunerability;
        HealthEventSystem.current.onConditionHit -= SetCondition;
        HealthEventSystem.current.onForceApply -= ApplyForce;
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

        hitStop.Stop(0.05f);
    }

    private float CheckDamageTypes(float damage, int damageType)
    {
        float dmg = damage;

        foreach (int type in resistanceHandler.damageImmunities)
        {
            if (type == damageType)
            {
                dmg = 0;
                break;
            }
        }
        if (dmg != 0)
        {
            foreach (int type in resistanceHandler.damageResistances)
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
    public void ApplyForce(string name, Vector3 direction, float magnitude)
    {
        if (gameObject.name == name)
        {
            if (invunarable)
                return;

            if (rb != null)
                rb.AddForce(direction.normalized * magnitude, ForceMode.Impulse);
        }
    }
}
