

public class EarthResistance : ResistanceEffect
{
    private void Start()
    {
        resistance = DamageTypesManager.Physical;
        resistanceAppearance = ResourceManager.Materials.Resistances.Physical;
    }

    public override string Name()
    {
        return "Physical Resistance";
    }
}
