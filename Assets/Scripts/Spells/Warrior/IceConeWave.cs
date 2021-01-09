using UnityEngine;

public class IceConeWave : ConeBurstSlash
{
    public override string skillName => "Ice Cone Wave";

    void Start()
    {
        damageType = DamageTypesManager.Cold;
        condition = ConditionsManager.Frozen;
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Sources.SwordEffects.Ice;
    }
}
