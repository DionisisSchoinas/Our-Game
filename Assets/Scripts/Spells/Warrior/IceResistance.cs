

public class IceResistance : ResistanceEffect
{

    private void Start()
    {
        resistance = DamageTypesManager.Cold;
        resistanceAppearance = ResourceManager.Materials.Resistances.Ice;
    }

    public override string Name()
    {
        return "Ice Resistance";
    }
}
