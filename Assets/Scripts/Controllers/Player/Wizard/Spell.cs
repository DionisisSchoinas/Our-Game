using UnityEngine;

public abstract class Spell : Skill
{
    [HideInInspector]
    public bool cancelled;

    public abstract bool channel { get; }
    public abstract void CastSpell(Transform firePoint, bool holding);
    public abstract void CancelCast();
    public abstract void WakeUp();
    public abstract ParticleSystem GetSource();

    protected bool isChanneling = false;


    public new void StartCooldown()
    {
        // If the spell wasn't cancelled
        if (!cancelled)
        {
            UIEventSystem.current.SkillCast(uniqueOverlayToWeaponAdapterId);
            onCooldown = true;
            startedCooldown = Time.time;
            Invoke(nameof(CooledDown), cooldown);
        }
    }
}
