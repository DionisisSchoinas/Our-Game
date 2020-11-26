using System.Collections.Generic;
using UnityEngine;

public class LightningRay : SpellTypeRay
{
    private void Start()
    {
        damageType = DamageTypesManager.Lightning;
        condition = ConditionsManager.Electrified;
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Lightning;
    }
    public override string Name()
    {
        return "Lightning Ray";
    }
}