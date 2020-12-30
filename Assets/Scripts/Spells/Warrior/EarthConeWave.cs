

public class EarthConeWave : ConeBurstSlash
{
    // Start is called before the first frame update
    void Start()
    {
        damageType = DamageTypesManager.Physical;
        condition = null;
    }

    public override string Name()
    {
        return "Earth Cone Wave";
    }
}
