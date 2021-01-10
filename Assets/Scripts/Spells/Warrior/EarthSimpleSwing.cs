using UnityEngine;

public class EarthSimpleSwing : ElementalSlash
{
    public override string skillName => "Earth Slash";

    void Start()
    {
        damageType = DamageTypesManager.Physical;
        condition = null;
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Sources.SwordEffects.Earth;
    }
}
