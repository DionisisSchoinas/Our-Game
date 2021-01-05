using UnityEngine;

public abstract class Spell : Skill
{
    [HideInInspector]
    public bool cancelled;

    protected Transform directionTransform;
    protected Vector3 spawnLocation;
    protected float destroyDistance;
    protected bool destroyBasedOnDistance;
    protected string casterName;

    public abstract bool channel { get; }
    public abstract void CastSpell(Transform firePoint, bool holding);
    public abstract void CancelCast();
    public abstract void WakeUp();
    public abstract ParticleSystem GetSource();

    protected bool isChanneling = false;

    public new void Awake()
    {
        base.Awake();
        destroyBasedOnDistance = false;
        cancelled = false;
    }

    public void FixedUpdate()
    {
        if (destroyBasedOnDistance)
        {
            if ((spawnLocation - transform.position).magnitude >= destroyDistance)
                Destroy(gameObject);
        }
    }

    public new void StartCooldown()
    {
        // If the spell wasn't cancelled
        if (!cancelled)
        {
            base.StartCooldown();
        }
    }


    public void CastSpell(Transform firePoint, bool holding, string caster)
    {
        casterName = caster;
        CastSpell(firePoint, holding);
    }

    public void CastSpell(Transform firePoint, Transform direction, bool holding, string caster)
    {
        directionTransform = direction;
        CastSpell(firePoint, holding, caster);
    }

    public void TransferData(Spell spell)
    {
        this.casterName = spell.casterName;
    }

    public void DesrtoyAfterDistanceTravelled(float distance)
    {
        spawnLocation = gameObject.transform.position;
        destroyDistance = distance;
        destroyBasedOnDistance = true;
    }

    protected Collider GetClosestCollider(Collider[] enemies)
    {
        Collider bestTarget = null;
        float closestDistanceSqr = 100f;
        Vector3 currentPosition = transform.position;
        foreach (Collider potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        return bestTarget;
    }
}
