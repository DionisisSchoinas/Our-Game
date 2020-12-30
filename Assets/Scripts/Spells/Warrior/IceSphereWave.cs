using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSphereWave : SphereBurst
{
    void Start()
    {
        damageType = DamageTypesManager.Cold;
        condition = ConditionsManager.Frozen;
    }

    public override string Name()
    {
        return "Ice Sphere Wave";
    }
}
