using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition
{
    public string name { get; private set; }
    public float duration { get; private set; }
    public float damage { get; private set; }
    public float damageTicks { get; private set; }
    public ParticleSystem effect { get; private set; }

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
