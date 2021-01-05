
public class LightningSimpleSwing : SimpleSlash
{
    public override string skillName => "Lightning Slash";

    void Start()
    {
        damageType = DamageTypesManager.Lightning;
        condition = ConditionsManager.Electrified;
    }
}
