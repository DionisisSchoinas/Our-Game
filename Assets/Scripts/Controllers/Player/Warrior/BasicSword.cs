using System.Collections.Generic;
using UnityEngine;

public abstract class BasicSword : Skill
{
    public float swingCooldown => 0.8f;

    public List<ComboStage> comboTrailTimings => new List<ComboStage>()
    {
        new ComboStage(0, 0.45f, 0.25f),
        new ComboStage(1, 0.22f, 0.25f),
        new ComboStage(2, 0.05f, 0.25f)
    };

    public abstract void Attack(PlayerMovementScriptWarrior controls, AttackIndicator indicator, SkinnedMeshRenderer playerMesh);

    public void StartSwingCooldown()
    {
        UIEventSystem.current.SkillCast(uniqueOverlayToWeaponAdapterId, swingCooldown * 0.5f);
    }
}
