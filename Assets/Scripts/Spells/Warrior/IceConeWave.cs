

public class IceConeWave : ConeBurstSlash
{
    void Start()
    {
        damageType = DamageTypesManager.Cold;
        condition = ConditionsManager.Frozen;
    }

    public override string Name()
    {
        return "Ice Cone Wave";
    }
}
