using UnityEngine;

public abstract class Spell : Skill
{
    public abstract void FireSimple(Transform firePoint);
    public abstract void FireHold(bool holding, Transform firePoint);
    public abstract void WakeUp();
    public abstract ParticleSystem GetSource();
}
