using System.Collections;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody rb;
    private Transform target;

    private float speed;
    private float damage;
    private float maxRotation;
    private float homingRange;
    private int damageType;
    private Condition condition;
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

        boost = false;
        spawnTime = Time.time;

        roamTime = 0f;

        startPosition = transform.position;
        transform.LookAt(transform.position + Random.onUnitSphere * 3f);
    }

    public void SetValues(float speed, float damage, float maxRotation, float homingRange, int damageType, Condition condition, string casterName, GameObject hitEffect)
    {
        this.speed = speed;
        this.damage = damage;
        this.maxRotation = maxRotation;
        this.homingRange = homingRange;
        this.damageType = damageType;
        this.condition = condition;
        this.casterName = casterName;
        this.hitEffect = hitEffect;

        tmpSpeed = speed * 3f;
    }

    private void FixedUpdate()
    {
        if (boost)
        {
            if (Time.time - spawnTime >= 0.5f)
            {
                boost = false;
                tmpSpeed = speed;
            }
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

    private void MoveAround()
    {
        if (roamTime == 0f)
        {
            orbitPoint = transform.position + Random.onUnitSphere * 2f;
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
        Collider[] collisions = Physics.OverlapSphere(transform.position, 1f);
        if (collisions.Length != 0)
        {
            Collider closest = FindClosestCollider(collisions);
            if (closest != null)
            {
                HealthEventSystem.current.TakeDamage(closest.gameObject.name, damage, damageType);
                if (condition != null)
                    if (Random.value <= 0.2f) HealthEventSystem.current.SetCondition(closest.gameObject.name, condition);

                Destroy(Instantiate(hitEffect, transform.position, transform.rotation), 2f);
                Destroy(gameObject);
            }
        }
    }
}