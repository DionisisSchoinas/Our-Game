using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireray : EnemySpell
{
    [SerializeField]
    private float damage = 5f;
    [SerializeField]
    private int damageTicksPerSecond = 5;

    private List<GameObject> collisions;
    private int damageablesLayer;

    private GameObject tmpLaser;

    private void Start()
    {
        collisions = new List<GameObject>();
        damageablesLayer = LayerMask.NameToLayer("Damageables");
        InvokeRepeating(nameof(DamageEnemies), 0f, 1f / damageTicksPerSecond);
    }


    public override void FireHold(bool holding, Transform firePoint)
    {
        if (holding)
        {
            tmpLaser = Instantiate(gameObject, firePoint);
            tmpLaser.SetActive(true);
        }
        else
        {
            Destroy(tmpLaser);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer.Equals(damageablesLayer))
        {
            if (!collisions.Contains(other.gameObject))
            {
                if (LineCasting.isLineClear(other.transform.position, transform.position, damageablesLayer))
                {
                    collisions.Add(other.gameObject);
                }
            }
        }
    }

    private void DamageEnemies()
    {
        foreach (GameObject gm in collisions)
        {
            if (gm != null)
                HealthEventSystem.current.TakeDamage(gm.name, damage);
        }
        collisions.Clear();
    }

    public override void WakeUp()
    {
    }

    public override void FireSimple(Transform firePoint)
    {
    }
}
