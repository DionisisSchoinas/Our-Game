
public class EarthSimpleSwing : SimpleSlash
{
    public override string Name => "Earth Slash";

    void Start()
    {
        damageType = DamageTypesManager.Physical;
        condition = null;
    }
}
