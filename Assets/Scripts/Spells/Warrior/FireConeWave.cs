

public class FireConeWave : ConeBurstSlash
{
    void Start()
    {
        damageType = DamageTypesManager.Fire;
        condition = ConditionsManager.Burning;
    }

    public override string Name()
    {
        return "Fire Cone Wave";
    }
}
