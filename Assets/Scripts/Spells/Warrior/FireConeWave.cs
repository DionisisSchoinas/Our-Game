using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireConeWave : ConeBurstSlash
{
    void Start()
    {
        damageType = DamageTypesManager.Fire;
        condition = ConditionsManager.Burning;
    }

    public override string Name()
    {
        return "Fire Cone Wave";
    }
}
