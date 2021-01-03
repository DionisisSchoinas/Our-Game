
public class LightningSimpleSwing : SimpleSlash
{
    public override string Name => "Lightning Slash";

    void Start()
    {
        damageType = DamageTypesManager.Lightning;
        condition = ConditionsManager.Electrified;
    }
}
