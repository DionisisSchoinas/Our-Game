using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireray : EnemySpell
{
    [SerializeField]
    private float damage = 5f;
    [SerializeField]
    private int damageTicksPerSecond = 5;

    private GameObject[] collisions;
    private Vector3 boxSize;

    private GameObject tmpLaser;

    private void Start()
    {
        boxSize = (new Vector3(3f, 5f, 18f)) / 2f;
        InvokeRepeating(nameof(Damage), 0f, 1f / damageTicksPerSecond);
    }

    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + Vector3.down + transform.forward * 9f, boxSize, transform.rotation, BasicLayerMasks.DamageableEntities);
        collisions = OverlapDetection.NoObstaclesLine(colliders, transform.position, BasicLayerMasks.IgnoreOnDamageRaycasts);
    }

    public override void FireHold(bool holding, Transform firePoint)
    {
        if (holding)
        {
            tmpLaser = Instantiate(gameObject, firePoint);
            tmpLaser.SetActive(true);
        }
        else
        {
            Destroy(tmpLaser);
        }
    }

    private void Damage()
    {
        if (collisions == null) return;

        foreach (GameObject gm in collisions)
        {
            if (gm != null)
            {
                HealthEventSystem.current.TakeDamage(gm, damage, DamageTypesManager.Fire);
                if (Random.value <= 0.25f / damageTicksPerSecond) HealthEventSystem.current.SetCondition(gm.name, ConditionsManager.Burning);
            }
        }
    }

    public override void WakeUp()
    {
    }

    public override void FireSimple(Transform firePoint)
    {
    }
}
