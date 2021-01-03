using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBall : SpellTypeBall
{
    public override string skillName => "Lightning Ball";

    private void Start()
    {
        InvokeRepeating(nameof(SpawnArcs), 0f, 0.1f);
    }

    private void SpawnArcs()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 10f, ~BasicLayerMasks.SpellsLayers);
        foreach (Collider c in colliders)
        {
            Instantiate(ResourceManager.Components.Arc, transform)
                .To(c.ClosestPoint(transform.position) + Random.insideUnitSphere)
                .SecondsAlive(0.2f)
                .Width(0.6f)
                .BreakPoints(25)
                .Enable();
        }
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Lightning;
    }
}
