
public class FireExplosion : Explosion
{
    private new void Start()
    {
        damageType = DamageTypesManager.Fire;
        condition = ConditionsManager.Burning;
        base.Start();
    }
}
