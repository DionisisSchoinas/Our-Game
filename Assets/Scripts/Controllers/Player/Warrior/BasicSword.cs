using UnityEngine;

public abstract class BasicSword : Skill
{
    public float swingCooldown => 0.7f;

    public abstract void Attack(PlayerMovementScriptWarrior controls, AttackIndicator indicator, SkinnedMeshRenderer playerMesh);

    public void StartSwingCooldown()
    {
        StartCooldownWithoutEvent(swingCooldown);
    }
}
