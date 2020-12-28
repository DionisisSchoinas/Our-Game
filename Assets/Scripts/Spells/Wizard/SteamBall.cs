using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamBall : SpellTypeBolt
{
    private void Start()
    {
        damage = 0f;
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Smoke;
    }

    public override string Name()
    {
        return "Steam Ball";
    }
}
