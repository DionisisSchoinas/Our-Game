using UnityEngine;

public abstract class BasicSword : Skill
{
    public float swingCooldown => 0.8f;

    public abstract void Attack(PlayerMovementScriptWarrior controls, AttackIndicator indicator, SkinnedMeshRenderer playerMesh);

    public void StartSwingCooldown()
    {
        UIEventSystem.current.SkillCast(uniqueOverlayToWeaponAdapterId, swingCooldown - 0.05f);
    }
}
