using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneBall : Spell
{
    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private GameObject explosion;
    [SerializeField]
    private Rigidbody rb;

    private SpellIndicatorController indicatorController;
    private GameObject path;

    private void Start()
    {
        path = GetComponentInChildren<ParticleSystem>().gameObject;
        path.SetActive(false);
        MovePath();
        path.SetActive(true);
    }

    void FixedUpdate()
    {
        rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.VelocityChange);
        MovePath();
    }

    private void MovePath()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            path.transform.position = hit.point;
        }
    }

    public override void FireSimple(Transform firePoint)
    {
        GameObject tmp = Instantiate(gameObject, firePoint.position, firePoint.rotation) as GameObject;
        Destroy(tmp, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //GameObject exp = Instantiate(explosion, path.transform.position + Vector3.up + transform.forward * 0.2f, transform.rotation) as GameObject;
        GameObject exp = Instantiate(explosion, transform.position + transform.forward * 0.2f + Vector3.down, transform.rotation) as GameObject;
        Destroy(exp, 4f);
        Destroy(gameObject);
    }
    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Earth;
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
