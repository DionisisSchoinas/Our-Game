using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellTypeRay : Spell
{
    public float damage = 10f;
    public int damageTicksPerSecond = 8;
    [HideInInspector]
    public int damageType;
    [HideInInspector]
    public Condition condition;

    private GameObject[] collisions;
    private Vector3 boxSize;
    private GameObject tmpRay;
    private SpellIndicatorController indicatorController;

    public override string type => "Ray";
    public override string skillName => "Ray";
    public override bool channel => true;
    public override float cooldown { get => 2f; }

    private void Awake()
    {
        boxSize = (new Vector3(3f, 5f, 18f)) / 2f;
        InvokeRepeating(nameof(Damage), 0f, 1f / damageTicksPerSecond);
    }

    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + Vector3.down + transform.forward * 9f, boxSize, transform.rotation, BasicLayerMasks.DamageableEntities);
        collisions = OverlapDetection.NoObstaclesLine(colliders, transform.position, BasicLayerMasks.IgnoreOnDamageRaycasts);
    }

    public override void CastSpell(Transform firePoint, bool holding)
    {
        if (holding)
        {
            if (tmpRay == null)
            {
                tmpRay = Instantiate(gameObject, firePoint);
                indicatorController = tmpRay.AddComponent<SpellIndicatorController>();
                indicatorController.SelectLocation(firePoint, 3f, 18f, SpellIndicatorController.SquareIndicator);
                tmpRay.SetActive(true);
            }
        }
        else
        {
            indicatorController.DestroyIndicator();
            Destroy(tmpRay);
        }
    }

    private void Damage()
    {
        if (collisions == null) return;

        foreach (GameObject gm in collisions)
        {
            if (gm != null)
            {
                HealthEventSystem.current.TakeDamage(gm.name, damage, damageType);
                if (condition != null)
                    if (Random.value <= 0.25f / damageTicksPerSecond) HealthEventSystem.current.SetCondition(gm.name, condition);
            }
        }
    }

    //------------------ Irrelevant ------------------
    public override ParticleSystem GetSource()
    {
        throw new System.NotImplementedException();
    }
    public override void WakeUp()
    {
    }
}
