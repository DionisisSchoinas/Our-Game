using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthSphereWave : SphereBurst
{
    void Start()
    {
        damageType = DamageTypesManager.Physical;
        condition = null;
    }

    public override string Name()
    {
        return "Earth Sphere Wave";
    }
}
