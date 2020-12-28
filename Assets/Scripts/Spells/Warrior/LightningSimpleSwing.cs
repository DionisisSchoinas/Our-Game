
public class LightningSimpleSwing : SimpleSlash
{
    void Start()
    {
        damageType = DamageTypesManager.Lightning;
        condition = ConditionsManager.Electrified;
    }

    public override string Name()
    {
        return "Lightning Slash";
    }
}
