

public class LightningResistance : ResistanceEffect
{
    public override string skillName => "Lightning Resistance";

    private void Start()
    {
        resistance = DamageTypesManager.Lightning;
        resistanceAppearance = ResourceManager.Materials.Resistances.Lightning;
    }
}
