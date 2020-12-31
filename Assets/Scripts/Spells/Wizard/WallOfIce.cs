using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallOfIce : SpellTypeWall
{
    public override string Name => "Ice Wall";

    private void Start()
    {
        doDamage = false;
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Ice;
    }
}
