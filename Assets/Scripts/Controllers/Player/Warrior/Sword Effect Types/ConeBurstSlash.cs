using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeBurstSlash : SwordEffect
{
    public float attackDelay = 0.5f;
    public float damage = 50f;
    public float coneWidth = 5f;
    public float coneLength = 10f;
    public float force = 5f;

    [HideInInspector]
    public int damageType = DamageTypesManager.Physical;
    [HideInInspector]
    public Condition condition = null;

    private SpellIndicatorController indicatorController;
    private ParticleSystem particles;
    private float attackAngle;

    private new void Awake()
    {
        base.Awake();
        // Unparent Wave particles
        particles = GetComponentInChildren<ParticleSystem>();
        particles.transform.parent = null;
        particles.transform.localScale = Vector3.one;

        // Calculate Cone angle
        Vector3 edge1 = Vector3.forward * coneLength + Vector3.right * coneWidth / 2f;
        Vector3 edge2 = Vector3.forward * coneLength - Vector3.right * coneWidth / 2f;
        attackAngle = Vector3.Angle(edge1, edge2);
    }

    private void OnDestroy()
    {
        Destroy(particles.gameObject);
    }

    public override void Attack(PlayerMovementScriptWarrior controls, AttackIndicator indicator)
    {
        StartCoroutine(PerformAttack(attackDelay, controls));
    }

    IEnumerator PerformAttack(float attackDelay, PlayerMovementScriptWarrior controls)
    {
        // Spawns Indicator
        indicatorController = gameObject.AddComponent<SpellIndicatorController>();
        indicatorController.SelectLocation(controls.transform, coneWidth, coneLength, SpellIndicatorController.ConeIndicator);



        yield return new WaitForSeconds(attackDelay);
        controls.sliding = true;

        // Spawns copy of particle system
        ParticleSystem parts = Instantiate(particles, controls.transform.position + controls.transform.forward * 2f, controls.transform.rotation);
        parts.Play();
        Destroy(parts.gameObject, 4f);

        // Find targets
        GameObject[] targets = FindTargets(controls.transform);

        foreach (GameObject visibleTarget in targets)
        {
            HealthEventSystem.current.TakeDamage(visibleTarget.name, damage, damageType);
            if (condition != null)
                if (Random.value <= 0.5f) HealthEventSystem.current.SetCondition(visibleTarget.name, condition);
            HealthEventSystem.current.ApplyForce(visibleTarget.name, controls.transform.forward, force);
        }
        yield return new WaitForSeconds(0.1f);
        controls.sliding = false;

        indicatorController.DestroyIndicator();
    }

    private GameObject[] FindTargets(Transform startingConePosition)
    {
        Vector3 boxCenter = startingConePosition.position + startingConePosition.forward * coneLength / 2f;
        Vector3 boxSize = new Vector3(coneLength, 5f, coneWidth);

        Collider[] boxCollisions = Physics.OverlapBox(boxCenter, boxSize / 2f, startingConePosition.rotation, BasicLayerMasks.DamageableEntities);
        GameObject[] notBlocked = OverlapDetection.NoObstaclesLine(boxCollisions, startingConePosition.position, BasicLayerMasks.IgnoreOnDamageRaycasts);
        List<GameObject> targets = new List<GameObject>();
        foreach (GameObject target in notBlocked)
        {
            Vector3 dirToTarget = (target.transform.position - startingConePosition.position);
            if (Vector3.Angle(startingConePosition.forward, dirToTarget) < attackAngle / 2 && !target.tag.Equals("Player"))
            {
                targets.Add(target);
            }
        }
        return targets.ToArray();
    }

    public override string Name()
    {
        return "Default Cone Slash";
    }
}
