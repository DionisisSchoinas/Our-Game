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
    [HideInInspector]
    public float activeCooldown;

    public abstract string type { get; }

    public abstract string skillName { get; }

    public abstract float cooldown { get; }

    public abstract float duration { get; }

    public void Awake()
    {
        onCooldown = false;
    }

    public void StartCooldown()
    {
        onCooldown = true;
        activeCooldown = cooldown;
        UIEventSystem.current.StartCooldown(this, cooldown);
        UIEventSystem.current.SkillCast(uniqueOverlayToWeaponAdapterId);
    }


    public void StartCooldownWithoutEvent(float delay)
    {
        onCooldown = true;
        activeCooldown = delay;
        UIEventSystem.current.StartCooldown(this, delay);
    }
}
