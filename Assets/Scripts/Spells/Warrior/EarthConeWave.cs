

public class EarthConeWave : ConeBurstSlash
{
    public override string Name => "Earth Cone Wave";

    void Start()
    {
        damageType = DamageTypesManager.Physical;
        condition = null;
    }
}
