using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneExplosion : Explosion
{
    private void Awake()
    {
        damageType = DamageTypesManager.Physical;
        condition = null;
    }
}
