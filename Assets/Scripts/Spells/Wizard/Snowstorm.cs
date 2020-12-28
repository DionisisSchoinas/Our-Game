using UnityEngine;

public class Snowstorm : SpellTypeStorm
{
    private Vector3 capsuleBottom;

    private void Start()
    {
        damageType = DamageTypesManager.Cold;
        condition = ConditionsManager.Frozen;

        transform.position += Vector3.down * 40f;
        capsuleBottom = transform.position + Vector3.down * 14f;
    }

    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapCapsule(capsuleBottom + Vector3.up * 50f, capsuleBottom, 14f, BasicLayerMasks.DamageableEntities);
        collisions = OverlapDetection.NoObstaclesHorizontal(colliders, capsuleBottom, BasicLayerMasks.IgnoreOnDamageRaycasts);
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Ice;
    }

    public override string Name()
    {
        return "Ice Storm";
    }
}
