

public class FireResistance : ResistanceEffect
{
    public override string skillName => "Fire Resistance";

    private void Start()
    {
        resistance = DamageTypesManager.Fire;
        resistanceAppearance = ResourceManager.Materials.Resistances.Fire;
    }
}
