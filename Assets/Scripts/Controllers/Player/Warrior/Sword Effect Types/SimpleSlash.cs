﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSlash : SwordEffect
{
    public float attackDelay = 0.5f;
    public float force = 5f;

    [HideInInspector]
    public int damageType = DamageTypesManager.Physical;
    [HideInInspector]
    public Condition condition = null;

    public override string type => "Simple Slash";
    public override string skillName => "Simple Slash";
    public override float cooldown => 10f;
    public override float duration => 10f;

    public override void Attack(PlayerMovementScriptWarrior controls, AttackIndicator indicator, SkinnedMeshRenderer playerMesh)
    {
        StartCoroutine(PerformAttack(attackDelay, controls, indicator));
    }

    IEnumerator PerformAttack(float attackDelay, PlayerMovementScriptWarrior controls, AttackIndicator indicator)
    {
        yield return new WaitForSeconds(attackDelay);
        controls.sliding = true;

        foreach (Transform visibleTarget in indicator.visibleTargets)
        {
            Debug.Log(visibleTarget.name);
            if (visibleTarget.name != controls.name)
            {
                HealthEventSystem.current.TakeDamage(visibleTarget.name, 30, damageType);
                if (condition != null)
                    if (Random.value <= 0.2f) HealthEventSystem.current.SetCondition(visibleTarget.name, condition);
                HealthEventSystem.current.ApplyForce(visibleTarget.name, controls.transform.forward, force);
            }
        }
        yield return new WaitForSeconds(0.1f);
        controls.sliding = false;
    }
}
