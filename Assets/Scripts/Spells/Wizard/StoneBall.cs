using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneBall : SpellTypeBall
{
    public override string skillName => "Stone Ball";

    private GameObject path;

    private void Start()
    {
        path = GetComponentInChildren<ParticleSystem>().gameObject;
        path.SetActive(false);
        MovePath();
        path.SetActive(true);
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();
        MovePath();
        rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.VelocityChange);
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

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Sources.Spells.Earth;
    }
}
