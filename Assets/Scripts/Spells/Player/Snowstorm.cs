using UnityEngine;

public class Snowstorm : SpellTypeStorm
{
    private void Start()
    {
        damageType = DamageTypesManager.Cold;
        condition = ConditionsManager.Frozen;
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Ice;
    }
    public override string Name()
    {
        return "Ice Storm";
    }
}
