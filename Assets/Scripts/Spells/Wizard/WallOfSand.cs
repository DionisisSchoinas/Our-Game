using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallOfSand : SpellTypeWall
{
    public override string skillName => "Sand Wall";

    private void Start()
    {
        doDamage = false;
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Sources.Earth;
    }
}
