

public class FireConeWave : ConeBurstSlash
{
    public override string skillName => "Fire Cone Wave";

    void Start()
    {
        damageType = DamageTypesManager.Fire;
        condition = ConditionsManager.Burning;
    }
}
