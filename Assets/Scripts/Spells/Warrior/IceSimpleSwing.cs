
public class IceSimpleSwing : SimpleSlash
{
    public override string skillName => "Ice Slash";

    void Start()
    {
        damageType = DamageTypesManager.Cold;
        condition = ConditionsManager.Frozen;
    }
}
