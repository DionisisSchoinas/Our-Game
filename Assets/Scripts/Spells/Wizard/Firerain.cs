using UnityEngine;

public class Firerain : SpellTypeStorm
{
    public override string Name => "Fire Storm";

    private void Start()
    {
        damageType = DamageTypesManager.Fire;
        condition = ConditionsManager.Burning;
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Fire;
    }
}