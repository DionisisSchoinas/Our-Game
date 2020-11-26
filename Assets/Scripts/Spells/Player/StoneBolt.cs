using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneBolt : SpellTypeBolt
{
    private void Start()
    {
        damageType = DamageTypesManager.Physical;
        condition = null;
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Earth;
    }
    public override string Name()
    {
        return "Stone Bolt";
    }
}
