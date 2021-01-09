using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fireball : SpellTypeBall
{
    public override string skillName => "Fire Ball";

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Sources.Spells.Fire;
    }
}
