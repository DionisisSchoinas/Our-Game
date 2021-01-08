using System.Collections.Generic;
using UnityEngine;

public class LightningRay : SpellTypeRay
{
    public override string skillName => "Lightning Ray";

    private void Start()
    {
        damageType = DamageTypesManager.Lightning;
        condition = ConditionsManager.Electrified;
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Sources.Lightning;
    }
}