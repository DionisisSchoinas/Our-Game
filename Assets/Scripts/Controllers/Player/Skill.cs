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
    }

    public void StartCooldown()
    {
        onCooldown = true;
        UIEventSystem.current.StartCooldown(this, cooldown);
        UIEventSystem.current.SkillCast(uniqueOverlayToWeaponAdapterId);
    }


    public void StartCooldownWithoutEvent(float delay)
    {
        onCooldown = true;
        UIEventSystem.current.StartCooldown(this, delay);
    }
}
