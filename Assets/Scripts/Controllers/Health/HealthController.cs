using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class HealthController : EntityResource
{
    public struct HealthControllerData
    {
        public bool invulnerable;
        public float health;
        //public NativeHashMap<int, NativeList<int>> resistances;
    }

    public HealthControllerData data;
    public List<int> resistances { get; private set; }
    public List<int> immunities { get; private set; }

    [SerializeField]
    private bool _invulnerable;
    public bool invulnerable
    {
        get => _invulnerable;
        set
        {
            _invulnerable = value;
            data.invulnerable = value;
        }
    }

    [SerializeField]
    private bool _respawn;
    public bool respawn
    {
        get => _respawn;
        set => _respawn = value;
    }

    public new float currentValue
    {
        get => base.currentValue;
        set
        {
            if (value <= 0)
            {
                if (respawn)
                    value = maxValue;
                else
                {
                    Destroy(gameObject);
                    return;
                }
            }

            base.currentValue = value;
            data.health = currentValue;
        }
    }

    private ConditionsHandler conditionsHandler;
    private ResistanceHandler resistanceHandler;
    private HitStop hitStop;

    public int healthSystemId;

    //temp
    Rigidbody rb;

    private new void Awake()
    {
        base.Awake();

        conditionsHandler = gameObject.AddComponent<ConditionsHandler>();
        resistanceHandler = gameObject.AddComponent<ResistanceHandler>();

        rb = gameObject.GetComponent<Rigidbody>() as Rigidbody;
        hitStop = FindObjectOfType<HitStop>();

        data = new HealthControllerData();
        resistances = new List<int>();
        immunities = new List<int>();
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

        if (data.health != currentValue)
        {
            data.health = currentValue;
        }

        healthSystemId = HealthEventSystem.current.Subscribe(this);
        HealthEventSystem.current.onChangeInvunerability += SetInvunerability;
        HealthEventSystem.current.onConditionHit += SetCondition;
        HealthEventSystem.current.onForceApply += ApplyForce;
        HealthEventSystem.current.onResistanceUpdate += UpdateResistances;
    }

    private void OnDestroy()
    {
        HealthEventSystem.current.UnSubscribe(this);
        HealthEventSystem.current.onChangeInvunerability -= SetInvunerability;
        HealthEventSystem.current.onConditionHit -= SetCondition;
        HealthEventSystem.current.onForceApply -= ApplyForce;
        HealthEventSystem.current.onResistanceUpdate -= UpdateResistances;
    }

    public void SetValues(float maxValue, float regenPerSecond, ResourceBar resourceBar, Color barColor, bool respawn, bool invulnerable)
    {
        SetValues(maxValue, regenPerSecond, resourceBar, barColor);
        this.respawn = respawn;
        this.invulnerable = invulnerable;

        data.invulnerable = this.invulnerable;
        data.health = this.currentValue;
    }

    private void UpdateResistances(string name, List<int> resistances)
    {
        if (gameObject.name == name)
        {
            this.resistances = resistances;
        }
    }

    private void UpdateImmunities(string name, List<int> immunities)
    {
        if (gameObject.name == name)
        {
            this.immunities = immunities;
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
                return;

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
