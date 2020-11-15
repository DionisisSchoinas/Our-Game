using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Spell
{
    [SerializeField]
    private Material idleMaterial;
    [SerializeField]
    private Vector3 hitShapeChangeOnHit;
    [SerializeField]
    private float hitDisturbanceRateOnHit;
    [SerializeField]
    private float hitDisturbanceDurationeOnHit;
    [SerializeField]
    private bool randomShapeChangeOnHit;

    private Vector3 defaultShapeChange = new Vector3(0.01f, 0.01f, 0.01f);
    private float defaultDisturbanceRate = 5f;

    private Vector3 randomizedHitShape;
    private GameObject tmpShield;
    private Transform center;

    private Collider[] colliders;
    private int damageablesLayer;

    private void Start()
    {
        //ResetMaterial();
        damageablesLayer = 1 << LayerMask.NameToLayer("Damageables");
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Shield"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Ground"), LayerMask.NameToLayer("Shield"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Damageables"), LayerMask.NameToLayer("Shield"));
    }

    private void FixedUpdate()
    {
        if (colliders != null)
        {
            foreach (Collider gm in colliders)
            {
                HealthEventSystem.current.SetInvunerable(gm.gameObject.name, false);
            }
        }
        colliders = Physics.OverlapSphere(transform.position, transform.localScale.x / 3f, damageablesLayer);
        foreach (Collider gm in colliders)
        {
            HealthEventSystem.current.SetInvunerable(gm.gameObject.name, true);
        }
    }

    private void OnDestroy()
    {
        foreach(Collider gm in colliders)
        {
            HealthEventSystem.current.SetInvunerable(gm.gameObject.name, false);
        }
    }

    /*
    public void Damaged()
    {
        if (randomShapeChangeOnHit)
            randomizedHitShape = new Vector3(Random.Range(0.01f, hitShapeChangeOnHit.x), Random.Range(0.01f, hitShapeChangeOnHit.y), Random.Range(0.01f, hitShapeChangeOnHit.z));
        else
            randomizedHitShape = hitShapeChangeOnHit;

        idleMaterial.SetFloat("_VertexOffsetFrequency", hitDisturbanceRateOnHit);
        idleMaterial.SetVector("_VertexOffsetDirection", randomizedHitShape);
        Invoke(nameof(ResetMaterial), hitDisturbanceDurationeOnHit);
    }

    void ResetMaterial()
    {
        idleMaterial.SetFloat("_VertexOffsetFrequency", defaultDisturbanceRate);
        idleMaterial.SetVector("_VertexOffsetDirection", defaultShapeChange);
    }
    */
    public override void FireSimple(Transform firePoint)
    {
    }

    public override void FireHold(bool holding, Transform firePoint)
    {
        if (holding)
        {
            tmpShield = Instantiate(gameObject, center);
        }
        else if (tmpShield != null)
        {
            Destroy(tmpShield);
        }
    }

    public override void SetIndicatorController(SpellIndicatorController controller)
    {
    }

    public override void WakeUp()
    {
        center = (FindObjectOfType<PlayerMovementScript>() as PlayerMovementScript).transform;
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Lightning;
    }
}
