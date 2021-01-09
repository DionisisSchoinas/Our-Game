using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icespike : SpellTypeBolt
{
    public override string skillName => "Ice Bolt";

    private void Start()
    {
        damageType = DamageTypesManager.Cold;
        condition = ConditionsManager.Frozen;
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Sources.Spells.Ice;
    }
}