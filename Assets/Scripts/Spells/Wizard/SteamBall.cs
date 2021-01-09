using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamBall : SpellTypeBolt
{
    public override string skillName => "Steam Ball";

    private void Start()
    {
        damage = 0f;
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Sources.Spells.Smoke;
    }
}
