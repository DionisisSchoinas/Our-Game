using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneRay : SpellTypeRay
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
        return "Stone Ray";
    }
}
