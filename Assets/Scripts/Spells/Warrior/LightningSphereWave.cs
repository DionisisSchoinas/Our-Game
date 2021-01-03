using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSphereWave : SphereBurst
{
    public override string Name => "Lightning Sphere Wave";

    void Start()
    {
        damageType = DamageTypesManager.Lightning;
        condition = ConditionsManager.Electrified;
    }
}
