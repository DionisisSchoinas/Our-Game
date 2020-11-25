using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBall : Spell
{
    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private GameObject explosion;
    [SerializeField]
    private Rigidbody rb;

    private SpellIndicatorController indicatorController;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnArcs), 0f, 0.1f);
    }
    private void SpawnArcs()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 10f, ~BasicLayerMasks.SpellsLayers);
        foreach (Collider c in colliders)
        {
            Instantiate(ResourceManager.Components.Arc, transform)
                .To(c.ClosestPoint(transform.position) + Random.insideUnitSphere)
                .SecondsAlive(0.2f)
                .Width(0.6f)
                .BreakPoints(25)
                .Enable();
        }
    }

    public override void FireSimple(Transform firePoint)
    {
        GameObject tmp = Instantiate(gameObject, firePoint.position, firePoint.rotation) as GameObject;
        Destroy(tmp, 5f);
    }

    void FixedUpdate()
    {
        if (rb != null)
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
        return ResourceManager.Default.Lightning;
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
