using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningWall : SpellTypeWall
{
    public override string Name => "Lightning Wall";

    private MeshRenderer[] pillars;

    private void Start()
    {
        doDamage = true;
        damageType = DamageTypesManager.Lightning;
        condition = ConditionsManager.Electrified;

        pillars = GetComponentsInChildren<MeshRenderer>();
        SpawnArcs();
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Lightning;
    }
    private void SpawnArcs()
    {
        Vector3 p1 = pillars[0].gameObject.transform.position;
        Vector3 p2 = pillars[1].gameObject.transform.position;

        for (int i = 2; i < 17; i++)
        {
            Instantiate(ResourceManager.Components.Arc, transform)
                .From(new Vector3(p1.x, i / 2f, p1.z))
                .To(new Vector3(p2.x, i / 2f, p2.z))
                .SecondsAlive(40f)
                .Width(0.6f)
                .BreakPoints(50)
                .EnabledSparks(false)
                .ChangeTicksPerSecond(15)
                .Enable();
        }
    }
}
