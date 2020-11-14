using System.Collections.Generic;
using UnityEngine;

public class LightningStorm : Spell
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
    private SpellIndicatorController indicatorController;

    private List<GameObject> collisions;
    private int damageablesLayer;

    void Start()
    {
        pickedSpot = false;
        collisions = new List<GameObject>();
        damageablesLayer = LayerMask.NameToLayer("Damageables");
        InvokeRepeating(nameof(DamageEnemies), 0f, 1f / damageTicksPerSecond);
    }

    public override void FireSimple(Transform firePoint)
    {
        if (pickedSpot)
        {
            pickedSpot = false;
            tmpStorm = Instantiate(gameObject);
            tmpStorm.transform.position = spawningLocation + Vector3.up * spawningHeight;
            tmpStorm.SetActive(true);
            Invoke(nameof(StopStorm), 10f);
        }
    }

    public override void FireHold(bool holding, Transform firePoint)
    {
        if (tmpStorm == null)
        {
            if (holding)
            {
                indicatorController.SelectLocation(20f, 15f);
                pickedSpot = false;
            }
            else
            {
                if (indicatorController != null)
                {
                    spawningLocation = indicatorController.LockLocation()[0];
                    pickedSpot = true;
                    Invoke(nameof(CancelSpell), 5f);
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

    private void StopStorm()
    {
        indicatorController.DestroyIndicator();
        Destroy(tmpStorm);
    }

    private void CancelSpell()
    {
        if (tmpStorm == null)
        {
            indicatorController.DestroyIndicator();
            pickedSpot = false;
        }
    }
    public override void SetIndicatorController(SpellIndicatorController controller)
    {
        indicatorController = controller;
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Lightning;
    }

    public override void WakeUp()
    {
    }
}
