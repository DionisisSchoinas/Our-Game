using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFirerain : EnemySpell
{
    [SerializeField]
    private float spawningHeight = 40f;
    [SerializeField]
    private float damage = 5f;
    [SerializeField]
    private int damageTicksPerSecond = 5;

    private GameObject tmpStorm;
    private Vector3 spawningLocation;
    private bool pickedSpot;

    private List<GameObject> collisions;
    private string[] damageablesLayer;

    private ParticleSystem tmpSource;

    void Start()
    {
        pickedSpot = false;
        collisions = new List<GameObject>();
        damageablesLayer = new string[] { "Damageables", "Spells" };
        InvokeRepeating(nameof(DamageEnemies), 1f, 1f / damageTicksPerSecond);
    }

    public override void FireSimple(Transform firePoint)
    {
        if (pickedSpot)
        {
            pickedSpot = false;
            tmpStorm = Instantiate(gameObject);
            tmpStorm.transform.position = spawningLocation + Vector3.up * spawningHeight;
            tmpStorm.SetActive(true);
            Invoke(nameof(StopStorm), 5f);
        }
    }

    public override void FireHold(bool holding, Transform firePoint)
    {
        if (tmpStorm == null)
        {
            if (holding)
            {
                pickedSpot = false;
                tmpSource = Instantiate(ResourceManager.Default.Fire, firePoint.position, firePoint.rotation);
            }
            else
            {
                spawningLocation = firePoint.forward * 15f - firePoint.up * 2f + firePoint.position;
                if (tmpSource != null ) Destroy(tmpSource.gameObject);
                pickedSpot = true;
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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Damageables")))
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

    private void StopStorm()
    {
        Destroy(tmpStorm);
    }

    public override void WakeUp()
    {
    }
}
