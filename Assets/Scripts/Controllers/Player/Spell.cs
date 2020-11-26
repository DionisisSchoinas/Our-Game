using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    public abstract void FireSimple(Transform firePoint);
    public abstract void FireHold(bool holding, Transform firePoint);
    public abstract void SetIndicatorController(SpellIndicatorController controller);
    public abstract void WakeUp();
    public abstract ParticleSystem GetSource();
    public abstract string Name();
}
