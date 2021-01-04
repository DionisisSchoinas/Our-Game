using System.Collections;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    [HideInInspector]
    public int uniqueOverlayToWeaponAdapterId;
    [HideInInspector]
    public bool onCooldown;
    [HideInInspector]
    public float cooldownPercentage;

    public abstract string type { get; }

    public abstract string skillName { get; }

    public abstract float cooldown { get; }

    public void Awake()
    {
        onCooldown = false;

        UIEventSystem.current.onSkillPicked += PickedSkill;
        UIEventSystem.current.onFreezeAllSkills += Freeze;
    }

    public void OnDestroy()
    {
        UIEventSystem.current.onSkillPicked -= PickedSkill;
        UIEventSystem.current.onFreezeAllSkills -= Freeze;
    }

    private void PickedSkill(int indexInAdapter)
    {
        StartCooldownWithoutEvent(OverlayControls.skillFreezeDuration);
    }

    private void Freeze(float delay)
    {
        StartCooldownWithoutEvent(delay);
    }

    public void StartCooldown()
    {
        UIEventSystem.current.SkillCast(uniqueOverlayToWeaponAdapterId);
        onCooldown = true;
        UIEventSystem.current.StartCooldown(this, cooldown);
    }


    public void StartCooldownWithoutEvent(float delay)
    {
        onCooldown = true;
        UIEventSystem.current.StartCooldown(this, delay);
    }
}
