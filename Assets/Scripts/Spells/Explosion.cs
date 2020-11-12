using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private float damage = 35f;

    private int damageablesLayer;

    private void Start()
    {
        damageablesLayer = LayerMask.NameToLayer("Damageables");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(damageablesLayer))
        {
            if (!Physics.Linecast(other.transform.position, transform.position, ~damageablesLayer))
            {
                other.SendMessage("Damage", damage);
            }
        }
    }
}
