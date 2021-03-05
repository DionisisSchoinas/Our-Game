using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private Collider col;
    private Rigidbody rb;
    [SerializeField]
    private float speed = 8f;

    private bool stuck;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        col = gameObject.GetComponent<Collider>();

        stuck = false;
    }

    public void SetCaster(Collider caster)
    {
        Physics.IgnoreCollision(col, caster);
    }

    public void FireSimple(Transform firePoint, Collider caster)
    {
        ProjectileScript scr = Instantiate(gameObject, firePoint.position + firePoint.forward * 0.5f, firePoint.rotation).GetComponent<ProjectileScript>();
        scr.SetCaster(caster);
        Destroy(scr.gameObject, 5f);
    }

    void FixedUpdate()
    {
        if (!stuck)
        {
            transform.RotateAround(transform.position, transform.forward, Time.deltaTime * 180f);
            rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.VelocityChange);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        stuck = true;
        // Stick to target
        int layer = collision.gameObject.layer;
        if (layer.Equals(BasicLayerMasks.EnemiesLayer) || layer.Equals(BasicLayerMasks.DamageablesLayer))
        {
            rb.transform.parent = collision.transform;
        }
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        // Disable colider
        col.enabled = false;
        // Move a bit forward
        transform.position += transform.forward / 3f;

        // Damage
        HealthEventSystem.current.TakeDamage(collision.gameObject.name, 15f, DamageTypesManager.Physical);

        Destroy(gameObject, 15f);
    }
}