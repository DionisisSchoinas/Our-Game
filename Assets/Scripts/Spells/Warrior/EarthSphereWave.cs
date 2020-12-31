using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthSphereWave : SphereBurst
{
    public override string Name => "Earth Sphere Wave";

    void Start()
    {
        damageType = DamageTypesManager.Physical;
        condition = null;
    }
}
