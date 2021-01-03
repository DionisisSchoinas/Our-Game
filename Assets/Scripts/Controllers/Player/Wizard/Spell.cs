using UnityEngine;

public abstract class Spell : Skill
{
    public abstract bool Channel { get; }
    public abstract void CastSpell(Transform firePoint, bool holding);
    public abstract void WakeUp();
    public abstract ParticleSystem GetSource();
}
