using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallOfIce : SpellTypeWall
{
    private void Start()
    {
        doDamage = false;
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Ice;
    }

    public override string Name()
    {
        return "Ice Wall";
    }
}
