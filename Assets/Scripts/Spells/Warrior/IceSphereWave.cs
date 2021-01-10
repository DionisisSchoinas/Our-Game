using UnityEngine;

public class IceSphereWave : SphereBurst
{
    public override string skillName => "Ice Sphere Wave";

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
