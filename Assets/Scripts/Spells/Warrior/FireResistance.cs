

public class FireResistance : ResistanceEffect
{

    private void Start()
    {
        resistance = DamageTypesManager.Fire;
        resistanceAppearance = ResourceManager.Materials.Resistances.Fire;
    }

    public override string Name()
    {
        return "Fire Resistance";
    }
}
