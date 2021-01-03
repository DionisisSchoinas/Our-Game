

public class FireResistance : ResistanceEffect
{
    public override string Name => "Fire Resistance";

    private void Start()
    {
        resistance = DamageTypesManager.Fire;
        resistanceAppearance = ResourceManager.Materials.Resistances.Fire;
    }
}
