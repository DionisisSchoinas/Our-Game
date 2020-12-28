using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iceball : SpellTypeBall
{
    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Ice;
    }
    public override string Name()
    {
        return "Ice Ball";
    }
}
