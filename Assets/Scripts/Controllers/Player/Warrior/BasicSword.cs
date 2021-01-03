using UnityEngine;

public abstract class BasicSword : Skill
{
    public abstract void Attack(PlayerMovementScriptWarrior controls, AttackIndicator indicator, SkinnedMeshRenderer playerMesh);
}
