using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereBurst : SwordEffect
{
    public float attackDelay = 0.5f;
    public float damage = 20f;
    public float sphereRadius = 15f;
    public float force = 10f;

    [HideInInspector]
    public int damageType = DamageTypesManager.Physical;
    [HideInInspector]
    public Condition condition = null;

    private SpellIndicatorController indicatorController;
    private ParticleSystem particles;

    public override string type => "Sphere Burst";
    public override string skillName => "Sphere Burst";
    public override float cooldown => 10f;

    private new void Awake()
    {
        base.Awake();
        // Unparent Wave particles
        particles = GetComponentInChildren<ParticleSystem>();
        particles.transform.parent = null;
        particles.transform.localScale = Vector3.one;
    }

    private void OnDestroy()
    {
        Destroy(particles.gameObject);
    }

    public override void Attack(PlayerMovementScriptWarrior controls, AttackIndicator indicator, SkinnedMeshRenderer playerMesh)
    {
        StartCoroutine(PerformAttack(attackDelay, controls));
    }

    IEnumerator PerformAttack(float attackDelay, PlayerMovementScriptWarrior controls)
    {
        // Spawns Indicator
        indicatorController = gameObject.AddComponent<SpellIndicatorController>();
        indicatorController.SelectLocation(controls.transform, sphereRadius * 2f, sphereRadius * 2f, SpellIndicatorController.CircleIndicator);
        indicatorController.DestroyIndicator(attackDelay + 0.1f);

        yield return new WaitForSeconds(attackDelay);
        controls.sliding = true;

        // Spawns copy of particle system
        ParticleSystem parts = Instantiate(particles, controls.transform.position + controls.transform.forward, controls.transform.rotation);
        parts.Play();
        Destroy(parts.gameObject, 4f);

        // Find targets
        GameObject[] targets = FindTargets(controls.transform);

        foreach (GameObject visibleTarget in targets)
        {
            if (visibleTarget.name != controls.name)
            {
                HealthEventSystem.current.TakeDamage(visibleTarget.name, damage, damageType);
                if (condition != null)
                    if (Random.value <= 0.5f) HealthEventSystem.current.SetCondition(visibleTarget.name, condition);
                HealthEventSystem.current.ApplyForce(visibleTarget.name, visibleTarget.transform.position - controls.transform.position, force);
            }
        }
        yield return new WaitForSeconds(0.1f);
        controls.sliding = false;
    }

    private GameObject[] FindTargets(Transform sphereCenter)
    {
        Collider[] sphereCollisions = Physics.OverlapSphere(sphereCenter.position, sphereRadius, BasicLayerMasks.DamageableEntities);
        GameObject[] notBlocked = OverlapDetection.NoObstaclesLine(sphereCollisions, sphereCenter.position, BasicLayerMasks.IgnoreOnDamageRaycasts);

        return notBlocked;
    }
}
