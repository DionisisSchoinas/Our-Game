
public class LightningExplosion : Explosion
{
    private new void Start()
    {
        damageType = DamageTypesManager.Lightning;
        condition = ConditionsManager.Electrified;
        base.Start();
    }
}