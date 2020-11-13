using System.Collections.Generic;
using UnityEngine;

public class LightningBolt : Spell
{
    [SerializeField]
    private float damagePerFrame = 5f;

    private GameObject tmpBolt;

    private List<GameObject> collisions;
    private int damageablesLayer;

    private SpellIndicatorController indicatorController;

    private void Start()
    {
        collisions = new List<GameObject>();
        damageablesLayer = LayerMask.NameToLayer("Damageables");
    }

    private void FixedUpdate()
    {
        foreach (GameObject gm in collisions)
        {
            if (gm != null)
                gm.SendMessage("Damage", damagePerFrame);
        }
        collisions.Clear();
    }

    public override void FireHold(bool holding, Transform firePoint)
    {
        if (holding)
        {
            tmpBolt = Instantiate(gameObject, firePoint);
            indicatorController.SelectLocation(firePoint, 3f, 18f);
            tmpBolt.SetActive(true);
        }
        else
        {
            Destroy(tmpBolt);
            indicatorController.DestroyIndicator();
        }
    }

    public override void WakeUp()
    {
        Start();
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

    public override void SetIndicatorController(SpellIndicatorController controller)
    {
        indicatorController = controller;
    }

    public override ParticleSystem GetSource()
    {
        return ((GameObject)Resources.Load("Spells/Default Lightning Source", typeof(GameObject))).GetComponent<ParticleSystem>();
    }

    public override void FireSimple(Transform firePoint)
    {
    }
}