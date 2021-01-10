
public class LightningExplosion : Explosion
{
    private void Awake()
    {
        damageType = DamageTypesManager.Lightning;
        condition = ConditionsManager.Electrified;
    }
}