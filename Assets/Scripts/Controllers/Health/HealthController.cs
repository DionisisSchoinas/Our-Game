using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : EntityResource
{
    [SerializeField]
    private bool invulnerable = false;

    public bool respawn = false;

    private ConditionsHandler conditionsHandler;
    private ResistanceHandler resistanceHandler;
    private HitStop hitStop;

    //temp
    Rigidbody rb;

    private new void Awake()
    {
        base.Awake();

        conditionsHandler = gameObject.AddComponent<ConditionsHandler>();
        resistanceHandler = gameObject.AddComponent<ResistanceHandler>();

        rb = gameObject.GetComponent<Rigidbody>() as Rigidbody;
        hitStop = FindObjectOfType<HitStop>();
    }

    private new void Start()
    {
        base.Start();

        if (currentValue != maxValue)
        {
            if (resourceBar != null)
                resourceBar.SetMaxValue(maxValue);

            currentValue = maxValue;
        }

        HealthEventSystem.current.onDamageTaken += TakeDamage;
        HealthEventSystem.current.onDamageIgnoreInvunarableTaken += TakeDamageIgnoreInvunarable;
        HealthEventSystem.current.onChangeInvunerability += SetInvunerability;
        HealthEventSystem.current.onConditionHit += SetCondition;
        HealthEventSystem.current.onForceApply += ApplyForce;
    }

    private void OnDestroy()
    {
        HealthEventSystem.current.onDamageTaken -= TakeDamage;
        HealthEventSystem.current.onDamageIgnoreInvunarableTaken -= TakeDamageIgnoreInvunarable;
        HealthEventSystem.current.onChangeInvunerability -= SetInvunerability;
        HealthEventSystem.current.onConditionHit -= SetCondition;
        HealthEventSystem.current.onForceApply -= ApplyForce;
    }

    public void SetValues(float maxValue, float regenPerSecond, ResourceBar resourceBar, Color barColor, bool respawn, bool invulnerable)
    {
        SetValues(maxValue, regenPerSecond, resourceBar, barColor);
        this.respawn = respawn;
        this.invulnerable = invulnerable;
    }

    public void Damage(float damage, int damageType)
    {
        if (!invulnerable)
        {
            DamageIgnoreInvunarable(damage, damageType);
        }
    }

    public void DamageIgnoreInvunarable(float damage, int damageType)
    {
        currentValue -= CheckDamageTypes(damage, damageType);

        if (currentValue <= 0)
        {
            if (!respawn)
                Destroy(gameObject);
            else
            {
                currentValue = maxValue;
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
            invulnerable = state;
        }
    }
    public void SetCondition(string name, Condition condition)
    {
        if (gameObject.name == name)
        {
            if (invulnerable)
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
            if (invulnerable)
                return;

            if (rb != null)
                rb.AddForce(direction.normalized * magnitude, ForceMode.Impulse);
        }
    }
    
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
