using UnityEngine;

public class SpellTypeWall : Spell
{
    public float damage = 5f;
    public int damageTicksPerSecond = 5;

    [HideInInspector]
    public bool doDamage;
    [HideInInspector]
    public int damageType;
    [HideInInspector]
    public Condition condition;

    private GameObject[] collisions;
    private Vector3 boxSize;

    private GameObject currentWall;
    private SpellIndicatorController indicatorController;
    private IndicatorResponse indicatorResponse;

    private GameObject tmpIndicatorHolder;

    public override string Type => "Wall";
    public override string Name => "Wall";
    public override bool Channel => true;

    private void Awake()
    {
        boxSize = (new Vector3(23f, 10f, 3f)) / 2f;
        InvokeRepeating(nameof(Damage), 0f, 1f / damageTicksPerSecond);
    }

    private void FixedUpdate()
    {
        if (doDamage)
        {
            Collider[] colliders = Physics.OverlapBox(transform.position + Vector3.up * 4f, boxSize, transform.rotation, BasicLayerMasks.DamageableEntities);
            collisions = OverlapDetection.NoObstaclesVertical(colliders, transform.position, BasicLayerMasks.IgnoreOnDamageRaycasts);
        }
    }

    public override void CastSpell(Transform firePoint, bool holding)
    {
        if (currentWall == null)
        {
            if (holding)
            {
                tmpIndicatorHolder = new GameObject();
                indicatorController = tmpIndicatorHolder.AddComponent<SpellIndicatorController>();
                indicatorController.SelectLocation(20f, 24f, 4f);
            }
            else
            {
                if (indicatorController != null)
                {
                    indicatorResponse = indicatorController.LockLocation();
                    Clear();
                    if (!indicatorResponse.isNull)
                    {
                        currentWall = Instantiate(gameObject);
                        currentWall.transform.position = Vector3.up * transform.localScale.y / 2 + indicatorResponse.centerOfAoe;
                        currentWall.transform.eulerAngles = indicatorResponse.spellRotation;
                        currentWall.SetActive(true);
                        Invoke(nameof(DeactivateWall), 10f);
                    }
                }
            }
        }
    }

    private void Damage()
    {
        if (collisions == null || !doDamage) return;

        foreach (GameObject gm in collisions)
        {
            if (gm != null)
            {
                HealthEventSystem.current.TakeDamage(gm.name, damage, damageType);
                if (condition != null)
                    if (Random.value <= 0.25f / damageTicksPerSecond) HealthEventSystem.current.SetCondition(gm.name, condition);
            }
        }
    }

    private void DeactivateWall()
    {
        Destroy(currentWall);
    }

    private void Clear()
    {
        indicatorController.DestroyIndicator();
        Destroy(tmpIndicatorHolder.gameObject);
    }

    //------------------ Irrelevant ------------------

    public override ParticleSystem GetSource()
    {
        throw new System.NotImplementedException();
    }
    public override void WakeUp()
    {
    }
}
