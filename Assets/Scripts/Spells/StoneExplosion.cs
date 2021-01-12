using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneExplosion : Explosion
{
    private new void Start()
    {
        damageType = DamageTypesManager.Physical;
        condition = null;
        base.Start();
    }
}
