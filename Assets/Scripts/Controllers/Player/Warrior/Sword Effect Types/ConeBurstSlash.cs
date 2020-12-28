using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeBurstSlash : SwordEffect
{
    public float attackDelay = 0.5f;
    [HideInInspector]
    public int damageType = DamageTypesManager.Physical;
    [HideInInspector]
    public Condition condition = null;

    /*
     * Use AttackIndicator logic with angle search and isntrad of overlap sphere use box to have flat edge
    */
}
