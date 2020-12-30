using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSphereWave : SphereBurst
{
    void Start()
    {
        damageType = DamageTypesManager.Lightning;
        condition = ConditionsManager.Electrified;
    }

    public override string Name()
    {
        return "Lightning Sphere Wave";
    }
}
