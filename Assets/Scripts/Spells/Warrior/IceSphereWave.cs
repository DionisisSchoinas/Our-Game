using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSphereWave : SphereBurst
{
    public override string skillName => "Ice Sphere Wave";

    void Start()
    {
        damageType = DamageTypesManager.Cold;
        condition = ConditionsManager.Frozen;
    }
}
