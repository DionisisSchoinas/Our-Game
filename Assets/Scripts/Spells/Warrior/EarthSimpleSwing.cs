
public class EarthSimpleSwing : SimpleSlash
{
    public override string skillName => "Earth Slash";

    void Start()
    {
        damageType = DamageTypesManager.Physical;
        condition = null;
    }
}
