using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningWall : Spell
{
    [SerializeField]
    private float damage = 5f;
    [SerializeField]
    private int damageTicksPerSecond = 5;

    private GameObject[] collisions;
    private Vector3 boxSize;

    private GameObject currentWall;
    private Vector3 spawningLocation;
    private Vector3 spellRotation;
    private bool pickedSpot;
    private SpellIndicatorController indicatorController;
    private IndicatorResponse indicatorResponse;

    private MeshRenderer[] pillars;

    private void Start()
    {
        pickedSpot = false;
        pillars = GetComponentsInChildren<MeshRenderer>();
        SpawnArcs();
        boxSize = (new Vector3(23f, 10f, 3f)) / 2f;
        InvokeRepeating(nameof(Damage), 0f, 1f / damageTicksPerSecond);
    }

    private void SpawnArcs()
    {
        Vector3 p1 = pillars[0].gameObject.transform.position;
        Vector3 p2 = pillars[1].gameObject.transform.position;

        for (int i=2; i < 17; i++)
        {
            Instantiate(ResourceManager.Components.Arc, transform)
                .From(new Vector3(p1.x, i / 2f, p1.z))
                .To(new Vector3(p2.x, i / 2f, p2.z))
                .SecondsAlive(40f)
                .Width(0.6f)
                .BreakPoints(50)
                .EnabledSparks(false)
                .ChangeTicksPerSecond(15)
                .Enable();
        }
    }

    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + Vector3.up * 4f, boxSize, transform.rotation, BasicLayerMasks.DamageableEntities);
        collisions = OverlapDetection.NoObstaclesVertical(colliders, transform.position + Vector3.up, BasicLayerMasks.IgnoreOnDamageRaycasts);
    }

    public override void FireSimple(Transform firePoint)
    {
        if (pickedSpot)
        {
            indicatorController.DestroyIndicator();
            pickedSpot = false;
            currentWall = Instantiate(gameObject);
            currentWall.transform.position = spawningLocation;
            currentWall.transform.eulerAngles = spellRotation;
            currentWall.SetActive(true);
            Invoke(nameof(DeactivateWall), 10f);
        }
    }

    public override void FireHold(bool holding, Transform firePoint)
    {
        if (currentWall == null)
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
                    indicatorResponse = indicatorController.LockLocation();
                    if (!indicatorResponse.isNull)
                    {
                        spawningLocation = indicatorResponse.centerOfAoe;
                        spellRotation = indicatorResponse.spellRotation;
                        pickedSpot = true;
                        Invoke(nameof(CancelSpell), indicatorController.indicatorDeleteTimer);
                    }
                }
            }
        }
    }

    private void Damage()
    {
        if (collisions == null) return;

        foreach (GameObject gm in collisions)
        {
            if (gm != null)
            {

                HealthEventSystem.current.TakeDamage(gm.name, damage, DamageTypesManager.Lightning);
                if (Random.value <= 0.25f / damageTicksPerSecond) HealthEventSystem.current.SetCondition(gm.name, ConditionsManager.Electrified);
            }
        }
    }

    private void DeactivateWall()
    {
        Destroy(currentWall);
    }

    private void CancelSpell()
    {
        if (currentWall == null && pickedSpot)
        {
            indicatorController.DestroyIndicator();
            pickedSpot = false;
        }
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Lightning;
    }

    public override void SetIndicatorController(SpellIndicatorController controller)
    {
        indicatorController = controller;
    }

    public override void WakeUp()
    {
    }
}
