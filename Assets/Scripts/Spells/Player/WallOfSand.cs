using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallOfSand : SpellTypeWall
{
    private void Start()
    {
        doDamage = false;
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Earth;
    }

    public override string Name()
    {
        return "Sand Wall";
    }
}
