using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFirerain : EnemySpell
{
    [SerializeField]
    private float damage = 5f;
    [SerializeField]
    private int damageTicksPerSecond = 5;

    private GameObject tmpStorm;
    private Vector3 spawningLocation;
    private bool pickedSpot;

    private GameObject[] collisions;
    private Vector3 capsuleTop;

    private ParticleSystem tmpSource;

    void Start()
    {
        pickedSpot = false;
        capsuleTop = transform.position + Vector3.up * 8f;
        InvokeRepeating(nameof(Damage), 1f, 1f / damageTicksPerSecond);
    }

    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapCapsule(capsuleTop, capsuleTop + Vector3.down * 60f, 14f, BasicLayerMasks.DamageableEntities);
        collisions = OverlapDetection.NoObstaclesVertical(colliders, capsuleTop, BasicLayerMasks.IgnoreOnDamageRaycasts);
    }

    public override void FireSimple(Transform firePoint)
    {
        if (pickedSpot)
        {
            pickedSpot = false;
            tmpStorm = Instantiate(gameObject);
            tmpStorm.transform.position = spawningLocation + Vector3.up * 40f;
            tmpStorm.SetActive(true);
            Invoke(nameof(StopStorm), 5f);
        }
    }

    public override void FireHold(bool holding, Transform firePoint)
    {
        if (tmpStorm == null)
        {
            if (holding)
            {
                pickedSpot = false;
                tmpSource = Instantiate(ResourceManager.Sources.Spells.Fire, firePoint.position, firePoint.rotation);
            }
            else
            {
                spawningLocation = firePoint.forward * 15f - firePoint.up * 2f + firePoint.position;
                if (tmpSource != null ) Destroy(tmpSource.gameObject);
                pickedSpot = true;
            }
        }
    }

    private void Damage()
    {
        if (collisions == null) return;

        foreach (GameObject gm in collisions)
        {
            if (gm != null)
            {
                HealthEventSystem.current.TakeDamage(gm.name, damage, DamageTypesManager.Fire);
                if (Random.value <= 0.2f / damageTicksPerSecond) HealthEventSystem.current.SetCondition(gm.name, ConditionsManager.Burning);
            }
        }
    }

    private void StopStorm()
    {
        Destroy(tmpStorm);
    }

    public override void WakeUp()
    {
    }
}
