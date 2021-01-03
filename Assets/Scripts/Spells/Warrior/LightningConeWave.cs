

public class LightningConeWave : ConeBurstSlash
{
    public override string skillName => "Lightning Cone Wave";

    void Start()
    {
        damageType = DamageTypesManager.Lightning;
        condition = ConditionsManager.Electrified;
    }
}
