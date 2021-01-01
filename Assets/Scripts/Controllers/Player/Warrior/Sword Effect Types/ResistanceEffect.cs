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

    private SkinnedMeshRenderer playerMesh;

    public override string Type => "Resistance";
    public override string Name => "No Resistance";

    public override void Attack(PlayerMovementScriptWarrior controls, AttackIndicator indicator, SkinnedMeshRenderer playerMesh)
    {
        StartCoroutine(PerformAttack(attackDelay, controls, playerMesh));
    }

    IEnumerator PerformAttack(float attackDelay, PlayerMovementScriptWarrior controls, SkinnedMeshRenderer playerMesh)
    {
        yield return new WaitForSeconds(attackDelay);

        Debug.Log("Resisant to : " + resistance + " impliment resistance event");

        if (resistanceAppearance != null)
        {
            playerMesh.materials = new Material[] { playerMesh.materials[0], resistanceAppearance };
        }
        else
        {
            playerMesh.materials = new Material[] { playerMesh.materials[0] };
        }

        this.playerMesh = playerMesh;
        Debug.Log("Mesh count : " + playerMesh.materials.Length);
    }

    /*
    private void OnDestroy()
    {
        playerMesh.materials = new Material[] { playerMesh.materials[0] };
    }
    */
}
