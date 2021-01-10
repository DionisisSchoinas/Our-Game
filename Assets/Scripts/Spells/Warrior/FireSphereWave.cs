using UnityEngine;

public class FireSphereWave : SphereBurst
{
    public override string skillName => "Fire Sphere Wave";

    void Start()
    {
        damageType = DamageTypesManager.Fire;
        condition = ConditionsManager.Burning;
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Sources.SwordEffects.Fire;
    }
}
