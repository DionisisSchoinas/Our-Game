using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icespike : SpellTypeBolt
{
    private void Start()
    {
        damageType = DamageTypesManager.Cold;
        condition = ConditionsManager.Frozen;
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Ice;
    }
    public override string Name()
    {
        return "Ice Bolt";
    }
}