using UnityEngine;

public abstract class Spell : Skill
{
    public abstract bool channel { get; }
    public abstract void CastSpell(Transform firePoint, bool holding);
    public abstract void WakeUp();
    public abstract ParticleSystem GetSource();
}
