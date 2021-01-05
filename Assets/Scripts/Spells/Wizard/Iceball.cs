using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iceball : SpellTypeBall
{

    public override string skillName => "Ice Ball";

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Ice;
    }
}
