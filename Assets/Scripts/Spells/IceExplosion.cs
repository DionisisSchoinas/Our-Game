using UnityEngine;

public class IceExplosion : MonoBehaviour
{
    [SerializeField]
    private float damage = 35f;
    [SerializeField]
    private float radius = 9f;

    private void Start()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, BasicLayerMasks.DamageableEntities);
        GameObject[] hitObjects = OverlapDetection.NoObstaclesLine(colliders, transform.position, BasicLayerMasks.IgnoreOnDamageRaycasts);
        foreach (GameObject gm in hitObjects)
        {
            Damage(gm);
        }
    }

    private void Damage(GameObject gm)
    {
        if (gm == null) return;

        HealthEventSystem.current.TakeDamage(gm.name, damage, DamageTypesManager.Cold);
        if (Random.value <= 0.5f) HealthEventSystem.current.SetCondition(gm.name, ConditionsManager.Frozen);
    }
}
