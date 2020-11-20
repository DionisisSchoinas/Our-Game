using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icespike : Spell
{
    [SerializeField]
    private float damage = 15f;
    [SerializeField]
    private float speed = 8f;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private GameObject explosionParticles;

    private SpellIndicatorController indicatorController;

    public override void FireSimple(Transform firePoint)
    {
        GameObject tmp = Instantiate(gameObject, firePoint.position + firePoint.forward * 0.5f, firePoint.rotation) as GameObject;
        Destroy(tmp, 5f);
    }

    void FixedUpdate()
    {
        rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Damageables")))
        {
            HealthEventSystem.current.TakeDamage(collision.gameObject.name, damage, DamageTypesManager.Cold);
            if (Random.value <= 0.2f) HealthEventSystem.current.SetCondition(collision.gameObject.name, ConditionsManager.Frozen);
            HealthEventSystem.current.ApplyForce(collision.gameObject.name, gameObject.transform.forward.normalized, 5f);
        }
        Destroy(Instantiate(explosionParticles, transform.position, transform.rotation), 1f);
        Destroy(gameObject);
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Ice;
    }

    public override void SetIndicatorController(SpellIndicatorController controller)
    {
        indicatorController = controller;
    }

    public override void FireHold(bool holding, Transform firePoint)
    {
    }

    public override void WakeUp()
    {
    }
}
