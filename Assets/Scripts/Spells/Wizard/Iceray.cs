using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iceray : SpellTypeRay
{
    public override string skillName => "Ice Ray";

    private void Start()
    {
        damageType = DamageTypesManager.Cold;
        condition = ConditionsManager.Frozen;
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Ice;
    }
}
