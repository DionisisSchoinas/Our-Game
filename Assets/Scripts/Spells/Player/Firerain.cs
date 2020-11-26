using UnityEngine;

public class Firerain : SpellTypeStorm
{
    private void Start()
    {
        damageType = DamageTypesManager.Fire;
        condition = ConditionsManager.Burning;
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Fire;
    }
    public override string Name()
    {
        return "Fire Storm";
    }
}