using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResistanceEffect : SwordEffect
{
    [HideInInspector]
    public int resistance = -1;
    [HideInInspector]
    public Material resistanceAppearance;

    public override string type => "Resistance";
    public override string skillName => "No Resistance";
    public override float cooldown => 20f;
    public override float duration => 10f;
    public override int comboPhaseMax => 1;
    public override bool instaCast => true;
    public override float manaCost => 10f;

    public override void Attack(PlayerMovementScriptWarrior controls, AttackIndicator indicator, SkinnedMeshRenderer playerMesh)
    {
        StartCoroutine(PerformAttack(comboTrailTimings[comboPhase].delayToFireSpell, controls, playerMesh));
    }

    IEnumerator PerformAttack(float attackDelay, PlayerMovementScriptWarrior controls, SkinnedMeshRenderer playerMesh)
    {
        if (instaCasting)
            yield return null;
        else
            yield return new WaitForSeconds(attackDelay);

        HealthEventSystem.current.ApplyResistance(controls.gameObject.name, playerMesh, resistanceAppearance, resistance, duration);
    }
}
