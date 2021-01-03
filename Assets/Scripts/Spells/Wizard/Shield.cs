﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Spell
{
    [SerializeField]
    private Material idleMaterial;
    [SerializeField]
    private Vector3 hitShapeChangeOnHit;
    [SerializeField]
    private float hitDisturbanceRateOnHit;
    [SerializeField]
    private float hitDisturbanceDurationeOnHit;
    [SerializeField]
    private bool randomShapeChangeOnHit;

    private GameObject tmpShield;
    private Transform center;

    private Collider[] colliders;
    private int damageablesLayer;

    public override string Type => "Shield";
    public override string Name => "Shield";
    public override bool Channel => true;

    private void Start()
    {
        //ResetMaterial();
        damageablesLayer = 1 << LayerMask.NameToLayer("Damageables");
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Shield"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Ground"), LayerMask.NameToLayer("Shield"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Damageables"), LayerMask.NameToLayer("Shield"));
    }

    private void FixedUpdate()
    {
        if (colliders != null)
        {
            foreach (Collider gm in colliders)
            {
                HealthEventSystem.current.SetInvunerable(gm.gameObject.name, false);
            }
        }
        colliders = Physics.OverlapSphere(transform.position, transform.localScale.x / 3f, damageablesLayer);
        foreach (Collider gm in colliders)
        {
            HealthEventSystem.current.SetInvunerable(gm.gameObject.name, true);
        }
    }

    private void OnDestroy()
    {
        foreach(Collider gm in colliders)
        {
            HealthEventSystem.current.SetInvunerable(gm.gameObject.name, false);
        }
    }

    public override void CastSpell(Transform firePoint, bool holding)
    {
        if (holding)
        {
            tmpShield = Instantiate(gameObject, center);
        }
        else if (tmpShield != null)
        {
            Destroy(tmpShield);
        }
    }

    public override void WakeUp()
    {
        center = (FindObjectOfType<PlayerMovementScriptWizard>() as PlayerMovementScriptWizard).transform;
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Lightning;
    }
}
