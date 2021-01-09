using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSphereWave : SphereBurst
{
    public override string skillName => "Lightning Sphere Wave";

    void Start()
    {
        damageType = DamageTypesManager.Lightning;
        condition = ConditionsManager.Electrified;
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Sources.SwordEffects.Lightning;
    }
}
