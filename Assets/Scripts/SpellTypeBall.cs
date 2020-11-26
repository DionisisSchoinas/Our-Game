using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellTypeBall : Spell
{
    public float speed = 8f;
    public GameObject explosion;

    private Rigidbody rb;
    private SpellIndicatorController indicatorController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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

    public override void FireSimple(Transform firePoint)
    {
        GameObject tmp = Instantiate(gameObject, firePoint.position, firePoint.rotation) as GameObject;
        Destroy(tmp, 5f);
    }

    public override void SetIndicatorController(SpellIndicatorController controller)
    {
        indicatorController = controller;
    }

    //------------------ Irrelevant ------------------
    public override void FireHold(bool holding, Transform firePoint)
    {
        throw new System.NotImplementedException();
    }

    public override void WakeUp()
    {
    }

    public override ParticleSystem GetSource()
    {
        throw new System.NotImplementedException();
    }

    public override string Name()
    {
        throw new System.NotImplementedException();
    }
}
