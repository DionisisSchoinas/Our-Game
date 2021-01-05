using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireray : SpellTypeRay
{
    public override string skillName => "Fire Ray";

    private void Start()
    {
        damageType = DamageTypesManager.Fire;
        condition = ConditionsManager.Burning;
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Fire;
    }
}