
public class IceSimpleSwing : SimpleSlash
{
    void Start()
    {
        damageType = DamageTypesManager.Cold;
        condition = ConditionsManager.Frozen;
    }

    public override string Name()
    {
        return "Ice Slash";
    }
}
