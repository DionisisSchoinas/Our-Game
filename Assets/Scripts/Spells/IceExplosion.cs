
public class IceExplosion : Explosion
{
    private void Awake()
    {
        damageType = DamageTypesManager.Cold;
        condition = ConditionsManager.Frozen;
    }
}
