﻿using UnityEngine;

public class Firewall : SpellTypeWall
{
    public override string Name => "Fire Wall";

    private void Start()
    {
        doDamage = true;
        damageType = DamageTypesManager.Fire;
        condition = ConditionsManager.Burning;
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Fire;
    }
}