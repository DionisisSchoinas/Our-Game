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

    private Quaternion rotation;
    private Vector3 orbitPoint;


    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();

        rb.mass = 0.1f;
        rb.useGravity = false;

        orbitPoint = transform.position;
        transform.LookAt(transform.position + Random.onUnitSphere * 3f);
    }

    public void SetValues(float speed, float damage, float maxRotation, float homingRange, int damageType, Condition condition, string casterName)
    {
        this.speed = speed;
        this.damage = damage;
        this.maxRotation = maxRotation;
        this.homingRange = homingRange;
        this.damageType = damageType;
        this.condition = condition;
        this.casterName = casterName;
    }

    private void FixedUpdate()
    {
        /*
        if (boost)
        {
            if (Time.time - spawnTime < 0.5f)
                return;

            if (Time.time - spawnTime >= 0.5f)
            {
                boost = false;
                rb.AddForce(new Vector3(Random.value, Random.value, Random.value) * 10f, ForceMode.Impulse);
            }
        }
        */
        if (target == null)
        {
            target = SearchForCloserTarget();
            MoveAround();
        }

        if (target != null)
        {
            // If target out of range
            if ((target.position - transform.position).sqrMagnitude > homingRange * homingRange)
            {
                orbitPoint = transform.position;
                target = null;
                return;
            }

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
        MoveToTarget(orbitPoint + Random.onUnitSphere * 2f);
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
        rb.velocity = transform.forward * speed;
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
                Destroy(gameObject);
            }
        }
    }

    private Vector3 SampleParabola(Vector3 start, Vector3 end, float height, float t)
    {
        float parabolicT = t * 2 - 1;
        if (Mathf.Abs(start.y - end.y) < 0.1f)
        {
            //start and end are roughly level, pretend they are - simpler solution with less steps
            Vector3 travelDirection = end - start;
            Vector3 result = start + t * travelDirection;
            result.y += (-parabolicT * parabolicT + 1) * height;
            return result;
        }
        else
        {
            //start and end are not level, gets more complicated
            Vector3 travelDirection = end - start;
            Vector3 levelDirecteion = end - new Vector3(start.x, end.y, start.z);
            Vector3 right = Vector3.Cross(travelDirection, levelDirecteion);
            Vector3 up = Vector3.Cross(right, travelDirection);
            if (end.y > start.y) up = -up;
            Vector3 result = start + t * travelDirection;
            result += ((-parabolicT * parabolicT + 1) * height) * up.normalized;
            return result;
        }
    }
}