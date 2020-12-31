

public class LightningConeWave : ConeBurstSlash
{
    public override string Name => "Lightning Cone Wave";

    void Start()
    {
        damageType = DamageTypesManager.Lightning;
        condition = ConditionsManager.Electrified;
    }
}
