using UnityEngine;

public class Firerain : SpellTypeStorm
{
    public override string skillName => "Fire Storm";

    private void Start()
    {
        damageType = DamageTypesManager.Fire;
        condition = ConditionsManager.Burning;
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Sources.Fire;
    }
}