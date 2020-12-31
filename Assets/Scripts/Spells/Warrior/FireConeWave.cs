

public class FireConeWave : ConeBurstSlash
{
    public override string Name => "Fire Cone Wave";

    void Start()
    {
        damageType = DamageTypesManager.Fire;
        condition = ConditionsManager.Burning;
    }
}
