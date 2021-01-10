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

    public abstract float duration { get; }

    public abstract float manaCost { get; }

    public abstract float instaCastDelay { get; }
    public abstract bool instaCast { get; }

    public void Awake()
    {
        onCooldown = false;
    }

    public void StartCooldown()
    {
        UIEventSystem.current.SkillCast(uniqueOverlayToWeaponAdapterId, cooldown);
    }

    public void StartCooldownWithoutEvent(float delay)
    {
        UIEventSystem.current.StartCooldown(this, delay);
    }
}
