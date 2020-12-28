
public class FireSimpleSwing : SimpleSlash
{
    void Start()
    {
        damageType = DamageTypesManager.Fire;
        condition = ConditionsManager.Burning;
    }

    public override string Name()
    {
        return "Fire Slash";
    }
}
