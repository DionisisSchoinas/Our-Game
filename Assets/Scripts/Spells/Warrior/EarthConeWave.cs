

public class EarthConeWave : ConeBurstSlash
{
    public override string skillName => "Earth Cone Wave";

    void Start()
    {
        damageType = DamageTypesManager.Physical;
        condition = null;
    }
}
