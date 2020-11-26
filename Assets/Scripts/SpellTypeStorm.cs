﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellTypeStorm : Spell
{
    public float damage = 5f;
    public int damageTicksPerSecond = 5;

    [HideInInspector]
    public int damageType;
    [HideInInspector]
    public Condition condition;

    private GameObject tmpStorm;
    private Vector3 spawningLocation;
    private bool pickedSpot;
    private SpellIndicatorController indicatorController;
    private IndicatorResponse indicatorResponse;

    private GameObject[] collisions;
    private Vector3 capsuleTop;

    private void Awake()
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
                HealthEventSystem.current.TakeDamage(gm.name, damage, damageType);
                if (condition != null)
                    if (Random.value <= 0.2f / damageTicksPerSecond) HealthEventSystem.current.SetCondition(gm.name, condition);
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

    //------------------ Irrelevant ------------------

    public override ParticleSystem GetSource()
    {
        throw new System.NotImplementedException();
    }
    public override void WakeUp()
    {
    }

    public override string Name()
    {
        throw new System.NotImplementedException();
    }
}
