using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Firebolt : Spell
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
            HealthEventSystem.current.TakeDamage(collision.gameObject.name, damage);
        }
        Destroy(Instantiate(explosionParticles, transform.position, transform.rotation), 1f);
        Destroy(gameObject);
    }

    public override ParticleSystem GetSource()
    {
        GameObject tmp = Instantiate(gameObject, Vector3.up * 1000, Quaternion.identity) as GameObject;
        Destroy(tmp, 0.1f);
        return tmp.transform.Find("Source").GetComponent<ParticleSystem>();
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
