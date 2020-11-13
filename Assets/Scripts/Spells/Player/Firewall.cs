using System.Collections.Generic;
using UnityEngine;

public class Firewall : Spell
{
    [SerializeField]
    private float damage = 5f;
    [SerializeField]
    private int damageTicksPerSecond = 5;

    private List<GameObject> collisions;
    private int damageablesLayer;

    private GameObject currentFireWall;
    private Vector3 spawningLocation;
    private Vector3 spellRotation;
    private bool pickedSpot;
    private SpellIndicatorController indicatorController;

    private void Start()
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
            indicatorController.DestroyIndicator();
            pickedSpot = false;
            currentFireWall = Instantiate(gameObject);
            currentFireWall.transform.position = Vector3.up * transform.localScale.y / 2 + spawningLocation;
            currentFireWall.transform.eulerAngles = spellRotation;
            currentFireWall.SetActive(true);
            Invoke(nameof(DeactivateWall), 10f);
        }
    }

    public override void FireHold(bool holding, Transform firePoint)
    {
        if (currentFireWall == null)
        {
            if (holding)
            {
                indicatorController.SelectLocation(20f, 24f, 4f);
                pickedSpot = false;
            }
            else
            {
                if (indicatorController != null)
                {
                    spawningLocation = indicatorController.LockLocation()[0];
                    spellRotation = indicatorController.LockLocation()[1];
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
                Vector3 belowEntity = new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z);
                if (LineCasting.isLineClear(other.transform.position, belowEntity, damageablesLayer))
                {
                    collisions.Add(other.gameObject);
                }
            }
        }
    }

    private void DeactivateWall()
    {
        Destroy(currentFireWall);
    }

    private void CancelSpell()
    {
        if (currentFireWall == null)
        {
            indicatorController.DestroyIndicator();
            pickedSpot = false;
        }
    }

    public override ParticleSystem GetSource()
    {
        return ((GameObject)Resources.Load("Spells/Default Fire Source", typeof(GameObject))).GetComponent<ParticleSystem>();
    }

    public override void SetIndicatorController(SpellIndicatorController controller)
    {
        indicatorController = controller;
    }

    public override void WakeUp()
    {
    }
}