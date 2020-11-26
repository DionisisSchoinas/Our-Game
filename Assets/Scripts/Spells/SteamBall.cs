using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamBall : SpellTypeBolt
{
    private void Start()
    {
        damage = 0f;
    }

    public new ParticleSystem GetSource()
    {
        return ResourceManager.Default.Smoke;
    }

    public new string Name()
    {
        return "Steam Ball";
    }
}
