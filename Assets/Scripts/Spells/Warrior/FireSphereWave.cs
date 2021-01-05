using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSphereWave : SphereBurst
{
    public override string skillName => "Fire Sphere Wave";

    void Start()
    {
        damageType = DamageTypesManager.Fire;
        condition = ConditionsManager.Burning;
    }
}
