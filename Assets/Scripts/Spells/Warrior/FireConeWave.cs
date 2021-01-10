using UnityEngine;

public class FireConeWave : ConeBurstSlash
{
    public override string skillName => "Fire Cone Wave";

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
