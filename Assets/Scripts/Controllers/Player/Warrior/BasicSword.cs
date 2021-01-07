using System.Collections.Generic;
using UnityEngine;

public abstract class BasicSword : Skill
{
    public float swingCooldown => 0.8f;

    public abstract int comboPhaseMax { get; }

    private int _comboPhase;
    public int comboPhase { 
        get
        {
            return _comboPhase;
        }
        set 
        {
            if (value >= comboTrailTimings.Count)
                _comboPhase = comboTrailTimings.Count - 1;
            else
                _comboPhase = value;
        }
    }

    public List<ComboStage> comboTrailTimings => new List<ComboStage>()
    {
        new ComboStage(0, 0.45f, 0.25f, 0.55f),
        new ComboStage(1, 0.22f, 0.25f, 0.34f),
        new ComboStage(2, 0.05f, 0.25f, 0.15f)
    };

    public abstract void Attack(PlayerMovementScriptWarrior controls, AttackIndicator indicator, SkinnedMeshRenderer playerMesh);

    public void StartSwingCooldown()
    {
        UIEventSystem.current.SkillCast(uniqueOverlayToWeaponAdapterId, swingCooldown * 0.5f);
    }
}
