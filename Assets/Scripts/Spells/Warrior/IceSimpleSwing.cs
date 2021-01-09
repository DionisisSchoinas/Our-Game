using UnityEngine;

public class IceSimpleSwing : SimpleSlash
{
    public override string skillName => "Ice Slash";

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
