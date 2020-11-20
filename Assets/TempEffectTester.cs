using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEffectTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(SetFire), 0f, 5f);
    }

    public void SetFire()
    {
        HealthEventSystem.current.SetCondition(gameObject.name, ConditionsManager.Burning);
    }
    
    public void SetElect()
    {
        HealthEventSystem.current.SetCondition(gameObject.name, ConditionsManager.Electrified);
    }
    public void SetFrozen()
    {
        HealthEventSystem.current.SetCondition(gameObject.name, ConditionsManager.Frozen);
    }
}
