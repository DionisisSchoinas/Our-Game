using UnityEngine;

public class EarthConeWave : ConeBurstSlash
{
    public override string skillName => "Earth Cone Wave";

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
