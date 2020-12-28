using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fireball : SpellTypeBall
{
    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Fire;
    }
    public override string Name()
    {
        return "Fire Ball";
    }
}
