

public class IceResistance : ResistanceEffect
{
    public override string skillName => "Ice Resistance";

    private void Start()
    {
        resistance = DamageTypesManager.Cold;
        resistanceAppearance = ResourceManager.Materials.Resistances.Ice;
    }
}
