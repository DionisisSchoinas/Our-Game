using UnityEngine;

public class LightningResistance : ResistanceEffect
{
    public override string skillName => "Lightning Resistance";

    private void Start()
    {
        resistance = DamageTypesManager.Lightning;
        resistanceAppearance = ResourceManager.Materials.Resistances.Lightning;
    }
    public override ParticleSystem GetSource()
    {
        return ResourceManager.Sources.SwordEffects.Lightning;
    }
}
