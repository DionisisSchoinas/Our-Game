using System.Collections.Generic;
using UnityEngine;

public class LightningBolt : Spell
{
    [SerializeField]
    private float damagePerFrame = 5f;

    private Transform simpleFirePoint;
    private Transform channelingFirePoint;
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

    public override void FireHold(bool holding)
    {
        if (holding)
        {
            indicatorController.SelectLocation(channelingFirePoint, 3f, 18f);
            tmpBolt.SetActive(true);
        }
        else
        {
            indicatorController.DestroyIndicator();
            tmpBolt.SetActive(false);
        }
    }

    public override void SetFirePoints(Transform point1, Transform point2)
    {
        simpleFirePoint = point1;
        channelingFirePoint = point2;
    }

    public override void WakeUp()
    {
        tmpBolt = Instantiate(gameObject, channelingFirePoint) as GameObject;
        tmpBolt.SetActive(false);
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
        return tmpBolt.transform.Find("Source").GetComponent<ParticleSystem>();
    }

    public override void FireSimple()
    {
    }
}