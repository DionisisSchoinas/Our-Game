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
    private SpellIndicatorController indicatorController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Damageables")))
        {
            HealthEventSystem.current.TakeDamage(collision.gameObject.name, damage, damageType);
            if (condition != null)
                if (Random.value <= 0.2f) HealthEventSystem.current.SetCondition(collision.gameObject.name, condition);
            HealthEventSystem.current.ApplyForce(collision.gameObject.name, gameObject.transform.forward.normalized, 5f);
        }
        Destroy(Instantiate(explosionParticles, transform.position, transform.rotation), 5f);
        Destroy(gameObject);
    }

    public override void FireSimple(Transform firePoint)
    {
        GameObject tmp = Instantiate(gameObject, firePoint.position + firePoint.forward * 0.5f, firePoint.rotation) as GameObject;
        Destroy(tmp, 5f);
    }

    public override void SetIndicatorController(SpellIndicatorController controller)
    {
        indicatorController = controller;
    }


    //------------------ Irrelevant ------------------

    public override void FireHold(bool holding, Transform firePoint)
    {
    }
    public override ParticleSystem GetSource()
    {
        throw new System.NotImplementedException();
    }

    public override void WakeUp()
    {
    }

    public override string Name()
    {
        throw new System.NotImplementedException();
    }
}
