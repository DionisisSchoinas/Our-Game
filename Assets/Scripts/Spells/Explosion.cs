using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float damage = 35f;
    public float radius = 9f;
    [HideInInspector]
    public int damageType;
    [HideInInspector]
    public Condition condition;

    private string casterName;

    protected void Start()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, BasicLayerMasks.DamageableEntities);
        GameObject[] hitObjects = OverlapDetection.NoObstaclesLine(colliders, transform.position, BasicLayerMasks.IgnoreOnDamageRaycasts);
        foreach (GameObject gm in hitObjects)
        {
            Damage(gm);
        }
    }

    public void SetName(string casterName)
    {
        this.casterName = casterName;
    }

    private void Damage(GameObject gm)
    {
        if (gm == null || gm.name == casterName)  return;

        HealthEventSystem.current.TakeDamage(gm.name, damage, damageType);
        if (condition != null && Random.value <= 0.5f) HealthEventSystem.current.SetCondition(gm.name, condition);
    }
}
