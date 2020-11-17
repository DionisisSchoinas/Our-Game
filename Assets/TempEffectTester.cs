using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEffectTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(SetFire), 2f);
        Invoke(nameof(SetElect), 5f);
        Invoke(nameof(SetFire), 6f);
    }

    public void SetFire()
    {
        HealthEventSystem.current.SetCondition(gameObject.name, ConditionsManager.Burning);
    }
    
    public void SetElect()
    {
        HealthEventSystem.current.SetCondition(gameObject.name, ConditionsManager.Electrified);
    }
}
