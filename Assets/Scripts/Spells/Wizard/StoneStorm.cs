using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneStorm : SpellTypeStorm
{
    public override string skillName => "Stone Storm";

    private Vector3 capsuleBottom;

    private void Start()
    {
        damageType = DamageTypesManager.Physical;
        condition = null;

        transform.position += Vector3.down * 40f;
        capsuleBottom = transform.position + Vector3.down * 14f;
    }

    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapCapsule(capsuleBottom + Vector3.up * 50f, capsuleBottom, 14f, BasicLayerMasks.DamageableEntities);
        collisions = OverlapDetection.NoObstaclesHorizontal(colliders, capsuleBottom, BasicLayerMasks.IgnoreOnDamageRaycasts);
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Earth;
    }
}
/*
{
    [SerializeField]
    private float damage = 5f;
    [SerializeField]
    private int damageTicksPerSecond = 5;

    private GameObject tmpStorm;
    private Vector3 spawningLocation;
    private bool pickedSpot;
    private SpellIndicatorController indicatorController;
    private IndicatorResponse indicatorResponse;

    private GameObject[] collisions;
    private Vector3 capsuleBottom;

    void Start()
    {
        pickedSpot = false;
        capsuleBottom = transform.position + Vector3.down * 14f;
        InvokeRepeating(nameof(Damage), 0f, 1f / damageTicksPerSecond);
    }

    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapCapsule(capsuleBottom + Vector3.up * 50f, capsuleBottom, 14f, BasicLayerMasks.DamageableEntities);
        collisions = OverlapDetection.NoObstaclesHorizontal(colliders, capsuleBottom, BasicLayerMasks.IgnoreOnDamageRaycasts);
    }

    public override void FireSimple(Transform firePoint)
    {
        if (pickedSpot)
        {
            pickedSpot = false;
            tmpStorm = Instantiate(gameObject);
            tmpStorm.transform.position = spawningLocation;
            tmpStorm.SetActive(true);
            Invoke(nameof(StopStorm), 10f);
        }
    }

    public override void FireHold(bool holding, Transform firePoint)
    {
        if (tmpStorm == null)
        {
            if (holding)
            {
                indicatorController.SelectLocation(20f, 15f);
                pickedSpot = false;
            }
            else
            {
                if (indicatorController != null)
                {
                    indicatorResponse = indicatorController.LockLocation();
                    if (!indicatorResponse.isNull)
                    {
                        spawningLocation = indicatorResponse.centerOfAoe;
                        pickedSpot = true;
                        Invoke(nameof(CancelSpell), indicatorController.indicatorDeleteTimer);
                    }
                }
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
                HealthEventSystem.current.TakeDamage(gm.name, damage, DamageTypesManager.Physical);
            }
        }
    }

    private void StopStorm()
    {
        indicatorController.DestroyIndicator();
        Destroy(tmpStorm);
    }

    private void CancelSpell()
    {
        if (tmpStorm == null)
        {
            indicatorController.DestroyIndicator();
            pickedSpot = false;
        }
    }
    public override void SetIndicatorController(SpellIndicatorController controller)
    {
        indicatorController = controller;
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Earth;
    }

    public override void WakeUp()
    {
    }
}
*/