

public class IceConeWave : ConeBurstSlash
{
    public override string Name => "Ice Cone Wave";

    void Start()
    {
        damageType = DamageTypesManager.Cold;
        condition = ConditionsManager.Frozen;
    }
}
