using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthSphereWave : SphereBurst
{
    public override string skillName => "Earth Sphere Wave";

    void Start()
    {
        damageType = DamageTypesManager.Physical;
        condition = null;
    }
}
