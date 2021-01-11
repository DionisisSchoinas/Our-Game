
public class IceExplosion : Explosion
{
    private new void Start()
    {
        damageType = DamageTypesManager.Cold;
        condition = ConditionsManager.Frozen;
        base.Start();
    }
}
