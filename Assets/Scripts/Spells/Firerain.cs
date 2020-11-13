using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firerain : Spell
{
    [SerializeField]
    private float spawningHeight = 40f;
    [SerializeField]
    private float damagePerFrame = 5f;

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
    }

    public override void WakeUp()
    {
        tmpStorm = Instantiate(gameObject) as GameObject;
        tmpStorm.SetActive(false);
        Start();
    }

    public override void FireSimple(Transform firePoint)
    {
        if (pickedSpot)
        {
            pickedSpot = false;
            tmpStorm.transform.position = spawningLocation + Vector3.up * spawningHeight;
            tmpStorm.SetActive(true);
            Invoke(nameof(StopStorm), 10f);
        }
    }

    public override void FireHold(bool holding, Transform firePoint)
    {
        if (!tmpStorm.activeInHierarchy)
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

    private void FixedUpdate()
    {
        foreach (GameObject gm in collisions)
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

    private void StopStorm()
    {
        indicatorController.DestroyIndicator();
        tmpStorm.SetActive(false);
    }

    private void CancelSpell()
    {
        if (!tmpStorm.activeSelf)
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
        return ((GameObject)Resources.Load("Spells/Default Fire Source", typeof(GameObject))).GetComponent<ParticleSystem>();
    }
}
