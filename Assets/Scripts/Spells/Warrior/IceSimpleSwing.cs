
public class IceSimpleSwing : SimpleSlash
{
    public override string Name => "Ice Slash";

    void Start()
    {
        damageType = DamageTypesManager.Cold;
        condition = ConditionsManager.Frozen;
    }
}
