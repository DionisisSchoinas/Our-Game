using System.Collections.Generic;
using UnityEngine;

public class Firewall : Spell
{
    [SerializeField]
    private float damagePerFrame = 5f;

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
    }

    public override void FireSimple(Transform firePoint)
    {
        if (pickedSpot)
        {
            indicatorController.DestroyIndicator();
            pickedSpot = false;
            currentFireWall.transform.position = Vector3.up * transform.localScale.y / 2 + spawningLocation;
            currentFireWall.transform.eulerAngles = spellRotation;
            currentFireWall.SetActive(true);
            Invoke(nameof(DeactivateWall), 10f);
        }
    }

    public override void FireHold(bool holding, Transform firePoint)
    {
        if (!currentFireWall.activeInHierarchy)
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
                collisions.Add(other.gameObject);
            }
        }
    }

    private void DeactivateWall()
    {
        currentFireWall.SetActive(false);
    }

    public override void WakeUp()
    {
        currentFireWall = Instantiate(gameObject) as GameObject;
        currentFireWall.SetActive(false);
        Start();
    }

    private void CancelSpell()
    {
        indicatorController.DestroyIndicator();
        pickedSpot = false;
    }

    public override ParticleSystem GetSource()
    {
        return ((GameObject)Resources.Load("Spells/Default Fire Source", typeof(GameObject))).GetComponent<ParticleSystem>();
    }

    public override void SetIndicatorController(SpellIndicatorController controller)
    {
        indicatorController = controller;
    }
}