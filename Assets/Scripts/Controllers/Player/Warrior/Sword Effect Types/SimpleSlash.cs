using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSlash : SwordEffect
{
    public float force = 5f;

    [HideInInspector]
    public int damageType = DamageTypesManager.Physical;
    [HideInInspector]
    public Condition condition = null;

    public override string type => "Simple Slash";
    public override string skillName => "Simple Slash";
    public override float cooldown => MeleeController.skillComboCooldown * 0.95f;
    public override float duration => 0f;

    public override void Attack(PlayerMovementScriptWarrior controls, AttackIndicator indicator, SkinnedMeshRenderer playerMesh)
    {
        StartCoroutine(PerformAttack(comboTrailTimings[comboPhase].delayToFireSpell, controls, indicator));
    }

    IEnumerator PerformAttack(float attackDelay, PlayerMovementScriptWarrior controls, AttackIndicator indicator)
    {
        yield return new WaitForSeconds(attackDelay);
        controls.sliding = true;

        foreach (Transform visibleTarget in indicator.visibleTargets)
        {
            if (visibleTarget.name != controls.name)
            {
                HealthEventSystem.current.TakeDamage(visibleTarget.gameObject, 30, damageType);
                if (condition != null)
                    if (Random.value <= 0.2f) HealthEventSystem.current.SetCondition(visibleTarget.name, condition);
                HealthEventSystem.current.ApplyForce(visibleTarget.name, controls.transform.forward, force);
                CameraShake.current.ShakeCamera(0.5f, 0.2f);
            }
        }
        yield return new WaitForSeconds(0.1f);
        controls.sliding = false;
    }
}
