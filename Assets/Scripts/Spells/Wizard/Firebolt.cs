﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firebolt : SpellTypeBolt
{
    private void Start()
    {
        damageType = DamageTypesManager.Fire;
        condition = ConditionsManager.Burning;
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Fire;
    }
    public override string Name()
    {
        return "Fire Bolt";
    }
}