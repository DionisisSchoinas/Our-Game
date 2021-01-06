using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellTypeStorm : Spell
{
    public float damage = 5f;
    public int damageTicksPerSecond = 5;

    [HideInInspector]
    public int damageType;
    [HideInInspector]
    public Condition condition;

    private GameObject tmpStorm;
    protected SpellIndicatorController indicatorController;
    protected IndicatorResponse indicatorResponse;
    protected GameObject tmpIndicatorHolder;

    [HideInInspector]
    public GameObject[] collisions;

    public override string type => "Storm";
    public override string skillName => "Storm";
    public override bool channel => true;
    public override float cooldown { get => 2f; }
    public override float duration { get => 10f; }

    public new void Awake()
    {
        base.Awake();
        cancelled = false;
        InvokeRepeating(nameof(Damage), 1f, 1f / damageTicksPerSecond);
    }

    private new void FixedUpdate()
    {
        Vector3 capsuleTop = transform.position + Vector3.up * 8f;
        Collider[] colliders = Physics.OverlapCapsule(capsuleTop, capsuleTop + Vector3.down * 60f, 14f, BasicLayerMasks.DamageableEntities);
        collisions = OverlapDetection.NoObstaclesVertical(colliders, capsuleTop, BasicLayerMasks.IgnoreOnDamageRaycasts);
    }

    public override void CastSpell(Transform firePoint, bool holding)
    {
        if (tmpStorm == null)
        {
            if (holding)
            {
                tmpIndicatorHolder = new GameObject();
                indicatorController = tmpIndicatorHolder.AddComponent<SpellIndicatorController>();
                indicatorController.SelectLocation(20f, 15f);
            }
            else
            {
                if (indicatorController != null)
                {
                    indicatorResponse = indicatorController.LockLocation();
                    if (!indicatorResponse.isNull && !cancelled)
                    {
                        tmpStorm = Instantiate(gameObject);
                        tmpStorm.transform.position = indicatorResponse.centerOfAoe + Vector3.up * 40f;
                        tmpStorm.SetActive(true);
                        Invoke(nameof(StopStorm), duration);
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
        if (collisions == null) return;

        foreach (GameObject gm in collisions)
        {
            if (gm != null)
            {
                HealthEventSystem.current.TakeDamage(gm.name, damage, damageType);
                if (condition != null)
                    if (Random.value <= 0.2f / damageTicksPerSecond) HealthEventSystem.current.SetCondition(gm.name, condition);
            }
        }
    }

    private void StopStorm()
    {
        Clear();
        Destroy(tmpStorm);
    }

    private void CancelSpell()
    {
        if (tmpStorm == null)
        {
            Clear();
        }
    }

    protected void Clear()
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
