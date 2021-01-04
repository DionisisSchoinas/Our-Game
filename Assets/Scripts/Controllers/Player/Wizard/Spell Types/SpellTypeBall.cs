using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellTypeBall : Spell
{
    public float speed = 8f;
    public GameObject explosion;
    [HideInInspector]
    public Rigidbody rb;
    private SpellIndicatorController indicatorController;

    public override string type => "Ball";
    public override string skillName => "Ball";
    public override bool channel => false;
    public override float cooldown { get => 2f; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cancelled = false;
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.VelocityChange);
    }

    public void OnCollisionEnter(Collision collision)
    {
        GameObject exp = Instantiate(explosion, transform.position + transform.forward * 0.2f, transform.rotation) as GameObject;
        Destroy(exp, 5f);
        Destroy(gameObject);
    }


    public override void CastSpell(Transform firePoint, bool holding)
    {
        if (cancelled)
        {
            cancelled = false;
            return;
        }

        if (!holding)
        {
            GameObject tmp = Instantiate(gameObject, firePoint.position, firePoint.rotation) as GameObject;
            Destroy(tmp, 5f);
        }
    }

    public override void CancelCast()
    {
        cancelled = true;
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
