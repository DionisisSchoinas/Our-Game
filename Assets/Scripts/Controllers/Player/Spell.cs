using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Spell : MonoBehaviour
{
    abstract public void FireSimple(Transform firePoint);
    abstract public void FireHold(bool holding, Transform firePoint);
    abstract public void SetIndicatorController(SpellIndicatorController controller);
    abstract public void WakeUp();
    abstract public ParticleSystem GetSource();
}
