using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private float damage = 35f;

    private string[] damageablesLayer;

    private void Start()
    {
        damageablesLayer = new string[] { "Damageables", "Spells" };
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Damageables")))
        {
            if (LineCasting.isLineClear(other.transform.position, transform.position, damageablesLayer))
            {
                HealthEventSystem.current.TakeDamage(other.gameObject.name, damage);
                if (Random.value <= 0.5f) HealthEventSystem.current.SetCondition(other.gameObject.name, ConditionsManager.Burning);
            }
        }
    }
}
