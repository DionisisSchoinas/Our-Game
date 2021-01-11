using UnityEngine;

public class FireSimpleSwing : ElementalSlash
{
    public override string skillName => "Fire Slash";

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
