using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class EnemySpell : MonoBehaviour
{
    abstract public void FireSimple(Transform firePoint);
    abstract public void FireHold(bool holding, Transform firePoint);
    abstract public void WakeUp();
}
