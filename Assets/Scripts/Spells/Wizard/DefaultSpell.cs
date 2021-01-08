using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultSpell : Spell
{
    public float spawnBulletCooldown = 1f;
    public GameObject homingMissile;

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

    private GameObject tmpSpell;

    private void Start()
    {
        Instantiate(ResourceManager.Sources.DefaultStationary, transform);
        InvokeRepeating(nameof(SpawnBullet), 0f, spawnBulletCooldown);
    }

    private void SpawnBullet()
    {
        Missile missile = Instantiate(homingMissile, transform.position + Random.onUnitSphere, transform.rotation).AddComponent<Missile>();
        missile.SetValues(speed, damage, maxRotation, homingRange, damageType, condition, casterName);
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
        return ResourceManager.Sources.Default;
    }

    public override void WakeUp()
    {
    }
}
