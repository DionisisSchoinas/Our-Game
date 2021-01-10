using UnityEngine;

public class EarthResistance : ResistanceEffect
{
    public override string skillName => "Physical Resistance";

    private void Start()
    {
        resistance = DamageTypesManager.Physical;
        resistanceAppearance = ResourceManager.Materials.Resistances.Physical;
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Sources.SwordEffects.Earth;
    }
}
