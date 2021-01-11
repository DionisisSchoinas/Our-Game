﻿using UnityEngine;

public class SpellTypeWall : Spell
{
    public float damage = 5f;
    public int damageTicksPerSecond = 5;

    [HideInInspector]
    public bool doDamage;
    [HideInInspector]
    public int damageType;
    [HideInInspector]
    public Condition condition;

    private GameObject[] collisions;
    private Vector3 boxSize;

    private GameObject currentWall;
    private SpellIndicatorController indicatorController;
    private IndicatorResponse indicatorResponse;

    private GameObject tmpIndicatorHolder;

    public override string type => "Wall";
    public override string skillName => "Wall";
    public override bool channel => true;
    public override float cooldown => 15f;
    public override float duration => 10f;
    public override float instaCastDelay => 0f;
    public override bool instaCast => false;
    public override float manaCost => 35f;

    public new void Awake()
    {
        base.Awake();
        boxSize = (new Vector3(23f, 10f, 3f)) / 2f;
        InvokeRepeating(nameof(Damage), 0f, 1f / damageTicksPerSecond);
    }

    private new void FixedUpdate()
    {
        if (doDamage)
        {
            Collider[] colliders = Physics.OverlapBox(transform.position + Vector3.up * 4f, boxSize, transform.rotation, BasicLayerMasks.DamageableEntities);
            collisions = OverlapDetection.NoObstaclesVertical(colliders, transform.position, BasicLayerMasks.IgnoreOnDamageRaycasts);
        }
    }

    public override void CastSpell(Transform firePoint, bool holding)
    {
        if (currentWall == null)
        {
            if (holding)
            {
                tmpIndicatorHolder = new GameObject();
                indicatorController = tmpIndicatorHolder.AddComponent<SpellIndicatorController>();
                indicatorController.SelectLocation(20f, 24f, 4f);
            }
            else
            {
                if (indicatorController != null)
                {
                    indicatorResponse = indicatorController.LockLocation();
                    if (!indicatorResponse.isNull && !cancelled)
                    {
                        ManaEventSystem.current.UseMana(manaCost);

                        currentWall = Instantiate(gameObject);
                        currentWall.transform.position = Vector3.up * transform.localScale.y / 2 + indicatorResponse.centerOfAoe;
                        currentWall.transform.eulerAngles = indicatorResponse.spellRotation;
                        currentWall.SetActive(true);
                        currentWall.GetComponent<Spell>().TransferData(this);
                        Invoke(nameof(DeactivateWall), duration);
                    }
                    else
                    {
                        if (cancelled)
                            cancelled = false;

                        Clear();
                    }
                }
            }
        }
    }
    public override void CancelCast()
    {
        cancelled = true;
    }

    private void Damage()
    {
        if (collisions == null || !doDamage) return;

        foreach (GameObject gm in collisions)
        {
            if (gm != null && gm.name != casterName)
            {
                HealthEventSystem.current.TakeDamage(gm.name, damage, damageType);
                if (condition != null)
                    if (Random.value <= 0.25f / damageTicksPerSecond) HealthEventSystem.current.SetCondition(gm.name, condition);
            }
        }
    }

    private void DeactivateWall()
    {
        Clear();
        Destroy(currentWall);
    }

    private void Clear()
    {
        if (indicatorController != null)
            indicatorController.DestroyIndicator();
        Destroy(tmpIndicatorHolder.gameObject);
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
