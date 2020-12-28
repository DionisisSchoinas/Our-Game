using UnityEngine;

public class Firewall : SpellTypeWall
{
    private void Start()
    {
        doDamage = true;
        damageType = DamageTypesManager.Fire;
        condition = ConditionsManager.Burning;
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Fire;
    }
    public override string Name()
    {
        return "Fire Wall";
    }
}