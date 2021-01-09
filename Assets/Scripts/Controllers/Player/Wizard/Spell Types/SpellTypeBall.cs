using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellTypeBall : Spell
{
    public float speed = 8f;
    public GameObject explosion;
    
    protected Rigidbody rb;
    private GameObject tmpIndicatorHolder;
    private SpellIndicatorController indicatorController;

    public override string type => "Ball";
    public override string skillName => "Ball";
    public override bool channel => false;
    public override float duration { get => 0f; }
    public override float cooldown { get => 20f; }
    public float range => 25f;
    public override float instaCastDelay => 0f;
    public override bool instaCast => false;

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
            // Ignore collisions with the caster
            if (colliders.Length == 1 && colliders[0].gameObject.name != casterName)
            {
                GameObject exp = Instantiate(explosion, transform.position + transform.forward * 0.2f, transform.rotation) as GameObject;
                CameraShake.current.ShakeCamera(1f, 1f);
                Destroy(exp, 5f);
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
            indicatorController.DisplayTargeting(firePoint, directionTransform, 0.5f, range);
        }
        else
        {
            Spell script = Instantiate(gameObject, firePoint.position + firePoint.forward * 0.5f, firePoint.rotation).GetComponent<Spell>();
            script.DesrtoyAfterDistanceTravelled(range - 1.5f);
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

    public override void WakeUp()
    {
    }

    public override ParticleSystem GetSource()
    {
        throw new System.NotImplementedException();
    }
}
