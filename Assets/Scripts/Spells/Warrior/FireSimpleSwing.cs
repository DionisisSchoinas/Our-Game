
public class FireSimpleSwing : SimpleSlash
{
    public override string Name => "Fire Slash";

    void Start()
    {
        damageType = DamageTypesManager.Fire;
        condition = ConditionsManager.Burning;
    }
}
