using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float damagePerFrame = 5f;

    private List<GameObject> collisions;
    private int damageablesLayer;

    private void Start()
    {
        collisions = new List<GameObject>();
        damageablesLayer = LayerMask.NameToLayer("Damageables");
    }

    private void FixedUpdate()
    {
        foreach(GameObject gm in collisions)
        {
            if (gm != null)
                gm.SendMessage("Damage", damagePerFrame);
        }
        collisions.Clear();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer.Equals(damageablesLayer))
        {
            if (!collisions.Contains(other.gameObject))
            {
                if (!Physics.Linecast(other.transform.position, transform.position, ~damageablesLayer))
                {
                    collisions.Add(other.gameObject);
                }
            }
        }
    }
}