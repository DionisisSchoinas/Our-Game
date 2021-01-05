
public class FireSimpleSwing : SimpleSlash
{
    public override string skillName => "Fire Slash";

    void Start()
    {
        damageType = DamageTypesManager.Fire;
        condition = ConditionsManager.Burning;
    }
}
