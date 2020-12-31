using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBolt : SpellTypeBolt
{
    public override string Name => "Lightning Bolt";

    private void Start()
    {
        damageType = DamageTypesManager.Lightning;
        condition = ConditionsManager.Electrified;
        InvokeRepeating(nameof(SpawnArcs), 0f, 0.1f);
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Lightning;
    }

    private void SpawnArcs()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5f, ~BasicLayerMasks.SpellsLayers);
        foreach (Collider c in colliders)
        {
            Instantiate(ResourceManager.Components.Arc, transform)
                .To(c.ClosestPoint(transform.position) + Random.insideUnitSphere)
                .SecondsAlive(0.2f)
                .Width(0.6f)
                .BreakPoints(15)
                .Enable();
        }
    }
}
