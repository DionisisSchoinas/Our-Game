using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellTypeBolt : Spell
{
    public float damage = 15f;
    public float speed = 40f;
    public GameObject explosionParticles;
    [HideInInspector]
    public int damageType;
    [HideInInspector]
    public Condition condition;

    private Rigidbody rb;
    private GameObject tmpIndicatorHolder;
    private SpellIndicatorController indicatorController;

    public override string type => "Bolt";
    public override string skillName => "Bolt";
    public override bool channel => false;
    public override float duration { get => 0f; }
    public override float cooldown { get => 2f; }
    public float range => 50f;
    public override float instaCastDelay => 0f;
    public override bool instaCast => false;
    public override float manaCost => 5f;

    public new void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
    }

    public new void FixedUpdate()
    {
        base.FixedUpdate();
        rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.VelocityChange);

        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);
        if (colliders.Length != 0)
        {
            Collider collision = GetClosestCollider(colliders);
            // Ignore collisions with the caster
            if (collision.gameObject.name != casterName)
            {
                HealthEventSystem.current.TakeDamage(collision.gameObject.name, damage, damageType);
                if (condition != null)
                    if (Random.value <= 0.2f) HealthEventSystem.current.SetCondition(collision.gameObject.name, condition);
                HealthEventSystem.current.ApplyForce(collision.gameObject.name, gameObject.transform.forward.normalized, 5f);

                CameraShake.current.ShakeCamera(0.1f, 0.2f);
                Destroy(Instantiate(explosionParticles, transform.position, transform.rotation), 5f);
                Destroy(gameObject);
            }
        }
    }

    public override void CastSpell(Transform firePoint, bool holding)
    {
        if (cancelled)
        {
            cancelled = false;
            Clear();
            return;
        }

        if (holding)
        {
            tmpIndicatorHolder = new GameObject();
            indicatorController = tmpIndicatorHolder.AddComponent<SpellIndicatorController>();
            indicatorController.DisplayTargeting(firePoint, directionTransform, 0.3f, range);
        }
        else
        {
            ManaEventSystem.current.UseMana(manaCost);

            Spell script = Instantiate(gameObject, firePoint.position + firePoint.forward * 0.5f, firePoint.rotation).GetComponent<Spell>();
            script.DesrtoyAfterDistanceTravelled(range - 2f);
            script.TransferData(this);
            Clear();
        }
    }

    public override void CancelCast()
    {
        cancelled = true;
        Clear();
    }

    protected void Clear()
    {
        if (indicatorController != null)
            indicatorController.DestroyIndicator();

        if (tmpIndicatorHolder != null)
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
