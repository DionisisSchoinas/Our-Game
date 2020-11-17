using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition
{
    public string name;
    public float duration;
    public float damage;
    public float damageTicks;
    public ParticleSystem effect;

    public Condition()
    {
        name = "Burning";
        duration = 5f;
        damage = 1f;
        damageTicks = 5f;
        effect = ResourceManager.Effects.Burning;
    }

    public Condition Name(string name)
    {
        this.name = name;
        return this;
    }
    public Condition Duration(float duration)
    {
        this.duration = duration;
        return this;
    }
    public Condition Damage(float damage)
    {
        this.damage = damage;
        return this;
    }
    public Condition DamageTicks(float damageTicks)
    {
        this.damageTicks = damageTicks;
        return this;
    }
    public Condition Effect(ParticleSystem effect)
    {
        this.effect = effect;
        return this;
    }
}
