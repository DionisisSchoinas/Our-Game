

public class LightningConeWave : ConeBurstSlash
{
    void Start()
    {
        damageType = DamageTypesManager.Lightning;
        condition = ConditionsManager.Electrified;
    }

    public override string Name()
    {
        return "Lightning Cone Wave";
    }
}
