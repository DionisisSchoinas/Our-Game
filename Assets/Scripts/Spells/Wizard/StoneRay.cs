using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneRay : SpellTypeRay
{
    public override string Name => "Stone Ray";

    private void Start()
    {
        damageType = DamageTypesManager.Physical;
        condition = null;
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Earth;
    }
}
