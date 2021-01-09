using UnityEngine;

public class LightningSimpleSwing : SimpleSlash
{
    public override string skillName => "Lightning Slash";

    void Start()
    {
        damageType = DamageTypesManager.Lightning;
        condition = ConditionsManager.Electrified;
    }
    public override ParticleSystem GetSource()
    {
        return ResourceManager.Sources.SwordEffects.Lightning;
    }
}
