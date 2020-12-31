

public class EarthResistance : ResistanceEffect
{
    public override string Name => "Physical Resistance";

    private void Start()
    {
        resistance = DamageTypesManager.Physical;
        resistanceAppearance = ResourceManager.Materials.Resistances.Physical;
    }
}
