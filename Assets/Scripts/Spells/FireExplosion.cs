
public class FireExplosion : Explosion
{
    private void Awake()
    {
        damageType = DamageTypesManager.Fire;
        condition = ConditionsManager.Burning;
    }
}
