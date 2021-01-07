using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResistanceEffect : SwordEffect
{
    public float attackDelay = 0.5f;
    [HideInInspector]
    public int resistance = -1;
    [HideInInspector]
    public Material resistanceAppearance;

    public override string type => "Resistance";
    public override string skillName => "No Resistance";
    public override float cooldown => 20f;
    public override float duration => 10f;
    public override int comboPhaseMax => 1;

    public override void Attack(PlayerMovementScriptWarrior controls, AttackIndicator indicator, SkinnedMeshRenderer playerMesh)
    {
        StartCoroutine(PerformAttack(attackDelay, controls, playerMesh));
    }

    IEnumerator PerformAttack(float attackDelay, PlayerMovementScriptWarrior controls, SkinnedMeshRenderer playerMesh)
    {
        yield return new WaitForSeconds(attackDelay);

        HealthEventSystem.current.ApplyResistance(controls.gameObject.name, playerMesh, resistanceAppearance, resistance, duration);

        /*
        Debug.Log("Resisant to : " + resistance + " impliment resistance event");

        if (resistanceAppearance != null)
        {
            playerMesh.materials = new Material[] { playerMesh.materials[0], resistanceAppearance };
            StartCoroutine(ResetSkinMesh());
        }
        else
        {
            playerMesh.materials = new Material[] { playerMesh.materials[0] };
        }

        this.playerMesh = playerMesh;
        */
    }
}
