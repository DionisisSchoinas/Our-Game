using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fireball : Spell
{
    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private GameObject explosion;
    [SerializeField]
    private Rigidbody rb;

    private SpellIndicatorController indicatorController;

    public override void FireSimple(Transform firePoint)
    {
        GameObject tmp = Instantiate(gameObject, firePoint.position, firePoint.rotation) as GameObject;
        Destroy(tmp, 5f);
    }

    void FixedUpdate()
    {
        rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject exp = Instantiate(explosion, transform.position + transform.forward * 0.2f, transform.rotation) as GameObject;
        Destroy(exp, 1f);
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
