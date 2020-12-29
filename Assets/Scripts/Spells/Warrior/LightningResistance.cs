

public class LightningResistance : ResistanceEffect
{
   
    private void Start()
    {
        resistance = DamageTypesManager.Lightning;
        resistanceAppearance = ResourceManager.Materials.Resistances.Lightning;
    }

    public override string Name()
    {
        return "Lightning Resistance";
    }
}
