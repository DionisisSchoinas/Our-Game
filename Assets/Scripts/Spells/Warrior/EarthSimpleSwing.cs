
public class EarthSimpleSwing : SimpleSlash
{
    void Start()
    {
        damageType = DamageTypesManager.Physical;
        condition = null;
    }

    public override string Name()
    {
        return "Earth Slash";
    }
}
