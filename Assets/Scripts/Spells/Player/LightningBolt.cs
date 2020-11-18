using System.Collections.Generic;
using UnityEngine;

public class LightningBolt : Spell
{
    [SerializeField]
    private float damage = 5f;
    [SerializeField]
    private int damageTicksPerSecond = 5;

    private GameObject tmpBolt;

    private List<GameObject> collisions;
    private string[] damageablesLayer;

    private SpellIndicatorController indicatorController;

    private void Start()
    {
        collisions = new List<GameObject>();
        damageablesLayer = new string[] { "Damageables", "Spells" };
        InvokeRepeating(nameof(DamageEnemies), 0f, 1f / damageTicksPerSecond);
    }

    public override void FireHold(bool holding, Transform firePoint)
    {
        if (holding)
        {
            tmpBolt = Instantiate(gameObject, firePoint);
            indicatorController.SelectLocation(firePoint, 3f, 18f);
            tmpBolt.SetActive(true);
        }
        else
        {
            Destroy(tmpBolt);
            indicatorController.DestroyIndicator();
        }
    }

    private void DamageEnemies()
    {
        foreach (GameObject gm in collisions)
        {
            if (gm != null)
            {
                HealthEventSystem.current.TakeDamage(gm.name, damage, DamageTypesManager.Lightning);
                if (Random.value <= 0.25f / damageTicksPerSecond) HealthEventSystem.current.SetCondition(gm.name, ConditionsManager.Electrified);
            }
        }
        collisions.Clear();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Damageables")))
        {
            if (!collisions.Contains(other.gameObject))
            {
                if (LineCasting.isLineClear(other.transform.position, transform.position, damageablesLayer))
                {
                    collisions.Add(other.gameObject);
                }
            }
        }
    }

    public override void SetIndicatorController(SpellIndicatorController controller)
    {
        indicatorController = controller;
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Lightning;
    }

    public override void FireSimple(Transform firePoint)
    {
    }

    public override void WakeUp()
    {
    }
}