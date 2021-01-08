using System.Collections;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody rb;

    private Transform target;
    private DefaultSpell parent;

    private float speed;
    private float maxRotation;
    private float homingRange;
    private string casterName;
    private GameObject hitEffect;

    private Quaternion rotation;
    private float tmpSpeed;
    private bool boost;
    private float spawnTime;

    private Vector3 orbitPoint;
    private float roamTime;
    private Vector3 startPosition;


    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();

        rb.mass = 0.1f;
        rb.useGravity = false;

        boost = true;
        spawnTime = Time.time;

        roamTime = 0f;

        startPosition = transform.position;
        //transform.LookAt(transform.position + Vector3.Cross(Vector3.up, Random.onUnitSphere));
        transform.LookAt(transform.position + Random.onUnitSphere);
    }

    public void SetValues(DefaultSpell parent, GameObject hitEffect, string casterName)
    {
        this.parent = parent;
        this.speed = parent.speed;
        this.maxRotation = parent.maxRotation;
        this.homingRange = parent.homingRange;
        this.casterName = casterName;
        this.hitEffect = hitEffect;

        tmpSpeed = 0f;
    }

    private void FixedUpdate()
    {
        if (boost)
        {
            if (spawnTime >= 0.6f)
            {
                boost = false;
                tmpSpeed = speed;
            }

            if (spawnTime >= 0.3f)
            {
                tmpSpeed = speed * 3f;
            }

            spawnTime += Time.deltaTime;
        }

        if (target == null)
        {
            target = SearchForCloserTarget();
            //MoveAround();
        }

        if (target != null)
        {
            // If target out of range
            if ((target.position - transform.position).sqrMagnitude > homingRange * homingRange)
            {
                roamTime = 0f;
                target = null;
                return;
            }
            /*
            if (!boost)
                tmpSpeed = speed;
            */
            MoveToTarget(target.position);
            CheckCollision();
        }
    }

    private Transform SearchForCloserTarget()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, homingRange, BasicLayerMasks.DamageableEntities);
        Collider target = FindClosestCollider(targets);

        if (target == null)
            return null;

        return target.transform;
    }
    /*
    private void MoveAround()
    {
        if (roamTime == 0f)
        {
            orbitPoint = transform.position + Random.onUnitSphere * 10f;

            roamTime += Time.deltaTime;
        }
        else if (roamTime <= 1f)
        {
            transform.position = Vector3.Lerp(startPosition, orbitPoint, roamTime);
            roamTime += Time.deltaTime;
        }
        else
            roamTime = 0;
    }
    */
    private Collider FindClosestCollider(Collider[] targets)
    {
        Collider bestTarget = null;
        float closestDistanceSqr = homingRange * homingRange * 2f ;
        foreach (Collider potentialTarget in targets)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - transform.position;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr && potentialTarget.gameObject.name != casterName)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        return bestTarget;
    }

    private void MoveToTarget(Vector3 position)
    {
        rotation = Quaternion.LookRotation(position - transform.position);
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, maxRotation));
        rb.velocity = transform.forward * tmpSpeed;
    }

    private void CheckCollision()
    {
        Collider[] collisions = Physics.OverlapSphere(transform.position, 0.5f);

        if (collisions.Length != 0)// && collisions[0].gameObject.name != casterName)
        {

            Collider closest = FindClosestCollider(collisions);
            if (closest != null)
            {
                RegisterHit(closest.gameObject);
                if (Random.value <= 0.3f) CameraShake.current.ShakeCamera(0.01f, 0.2f);
                Destroy(Instantiate(hitEffect, transform.position, transform.rotation), 1f);
                Destroy(gameObject);
            }
        }
    }

    private void RegisterHit(GameObject target)
    {
        parent.hitTargets.Add(target);
    }
}