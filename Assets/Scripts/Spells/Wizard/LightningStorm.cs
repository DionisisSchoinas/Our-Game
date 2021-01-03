﻿using System.Collections.Generic;
using UnityEngine;

public class LightningStorm : SpellTypeStorm
{
    public override string Name => "Lightning Storm";

    private void Start()
    {
        damageType = DamageTypesManager.Lightning;
        condition = ConditionsManager.Electrified;
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Lightning;
    }
}