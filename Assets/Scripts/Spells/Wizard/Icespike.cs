using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icespike : SpellTypeBolt
{
    public override string Name => "Ice Bolt";

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