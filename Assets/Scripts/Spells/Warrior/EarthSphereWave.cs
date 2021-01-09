using UnityEngine;

public class EarthSphereWave : SphereBurst
{
    public override string skillName => "Earth Sphere Wave";

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
