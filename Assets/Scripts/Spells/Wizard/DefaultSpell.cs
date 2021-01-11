using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultSpell : Spell
{
    public float spawnBulletCooldown = 1f;
    public float damageTicksPerSeconds = 5f;
    public GameObject homingMissile;
    public GameObject hitEffect;

    [HideInInspector]
    public List<GameObject> hitTargets;

    public float speed = 20f;
    public float damage = 5f;
    public float maxRotation = 1f;
    public float homingRange = 20f;

    private int damageType = DamageTypesManager.Physical;
    private Condition condition = null;

    public override bool channel => true;
    public override string type => "Homing Bolts";
    public override string skillName => "Default Spell";
    public override float cooldown => 2f;
    public override float duration => 0f;
    public override float instaCastDelay => 0f;
    public override bool instaCast => false;
    public override float manaCost => 0f;


    private GameObject tmpSpell;

    private void Start()
    {
        Instantiate(ResourceManager.Sources.Spells.DefaultStationary, transform);
        InvokeRepeating(nameof(SpawnBullet), 0f, spawnBulletCooldown);
        InvokeRepeating(nameof(DoDamage), 0f, 1f / damageTicksPerSeconds);
    }

    private void DoDamage()
    {
        foreach (GameObject target in hitTargets)
        {
            HealthEventSystem.current.TakeDamageWithoutEvent(target, damage, damageType);

            if (condition != null)
                if (Random.value <= 0.1f) HealthEventSystem.current.SetCondition(target.name, condition);
        }
        hitTargets.Clear();
    }

    private void SpawnBullet()
    {
        Missile missile = Instantiate(homingMissile, transform.position + Random.onUnitSphere / 2f, transform.rotation).AddComponent<Missile>();
        missile.SetValues(this, hitEffect, casterName);
        Destroy(missile.gameObject, 3f);
    }

    public override void CastSpell(Transform firePoint, bool holding)
    {
        if (holding)
        {
            tmpSpell = Instantiate(gameObject, firePoint);
            Spell script = tmpSpell.GetComponent<Spell>();
            script.TransferData(this);
        }
        else
        {
            if (cancelled)
                cancelled = false;

            if (tmpSpell != null)
                Destroy(tmpSpell);
        }
    }

    public override void CancelCast()
    {
        cancelled = true;
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Sources.Spells.Default;
    }

    public override void WakeUp()
    {
    }
}
