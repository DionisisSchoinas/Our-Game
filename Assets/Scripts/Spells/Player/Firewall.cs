using System.Collections.Generic;
using UnityEngine;

public class Firewall : Spell
{
    [SerializeField]
    private float damage = 5f;
    [SerializeField]
    private int damageTicksPerSecond = 5;

    private GameObject[] collisions;
    private Vector3 boxSize;

    private GameObject currentFireWall;
    private Vector3 spawningLocation;
    private Vector3 spellRotation;
    private bool pickedSpot;
    private SpellIndicatorController indicatorController;
    private IndicatorResponse indicatorResponse;

    private void Start()
    {
        pickedSpot = false;
        boxSize = (new Vector3(23f, 10f, 3f)) / 2f;
        InvokeRepeating(nameof(Damage), 0f, 1f / damageTicksPerSecond);
    }

    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + Vector3.up * 4f, boxSize, transform.rotation, BasicLayerMasks.DamageableEntities);
        collisions = OverlapDetection.NoObstaclesVertical(colliders, transform.position, BasicLayerMasks.IgnoreOnDamageRaycasts);
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
                HealthEventSystem.current.TakeDamage(gm.name, damage, DamageTypesManager.Fire);
                if (Random.value <= 0.25f / damageTicksPerSecond) HealthEventSystem.current.SetCondition(gm.name, ConditionsManager.Burning);
            }
        }
    }

    private void DeactivateWall()
    {
        Destroy(currentFireWall);
    }

    private void CancelSpell()
    {
        if (currentFireWall == null && pickedSpot)
        {
            indicatorController.DestroyIndicator();
            pickedSpot = false;
        }
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Fire;
    }

    public override void SetIndicatorController(SpellIndicatorController controller)
    {
        indicatorController = controller;
    }

    public override void WakeUp()
    {
    }
}