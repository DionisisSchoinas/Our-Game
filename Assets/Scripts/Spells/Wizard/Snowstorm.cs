using UnityEngine;

public class Snowstorm : SpellTypeStorm
{
    public override string skillName => "Ice Storm";

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
        return ResourceManager.Sources.Spells.Ice;
    }
}
