using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fireball : SpellTypeBall
{
    public override string Name => "Fire Ball";

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Fire;
    }
}
