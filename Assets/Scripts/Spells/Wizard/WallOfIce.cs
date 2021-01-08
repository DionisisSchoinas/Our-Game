using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallOfIce : SpellTypeWall
{
    public override string skillName => "Ice Wall";

    private void Start()
    {
        doDamage = false;
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Sources.Ice;
    }
}
