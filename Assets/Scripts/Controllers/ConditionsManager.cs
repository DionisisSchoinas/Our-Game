using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionsManager
{
    public static Condition Burning = new Condition();
    public static Condition Electrified = new Condition().Name("Electrified").Effect(ResourceManager.Effects.Electrified);
}
