using UnityEngine;

public class MeteorShower : Spell
{
    [SerializeField]
    private GameObject meteorPrefab;
    [SerializeField]
    private float spawningRadius;
    [SerializeField]
    private float spawningHeight;
    [SerializeField]
    private float projectilesPerSecond;

    private Vector3 spellLocation;
    private bool pickedSpot;
    private Vector3 spawningLocation;
    private SpellIndicatorController indicatorController;
    private IndicatorResponse indicatorResponse;
    private bool firing;

    private GameObject tmpIndicatorHolder;

    public override string Type => "Meteors";
    public override string Name => "Meteors";

    private void Start()
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Spells"), LayerMask.NameToLayer("SpellIgnoreLayer"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("SpellIgnoreLayer"), LayerMask.NameToLayer("SpellIgnoreLayer"));

        pickedSpot = false;
        firing = false;
    }

    public override void WakeUp()
    {
        Start();
    }

    public override void FireSimple(Transform firePoint)
    {
        if (pickedSpot)
        {
            pickedSpot = false;
            spellLocation = spawningLocation + Vector3.up * spawningHeight;
            firing = true;
            Fire();
            Invoke(nameof(StopFiring), 10f);
        }
    }

    public override void FireHold(bool holding, Transform firePoint)
    {
        if (!firing)
        {
            if (holding)
            {
                tmpIndicatorHolder = new GameObject();
                indicatorController = tmpIndicatorHolder.AddComponent<SpellIndicatorController>();
                indicatorController.SelectLocation(20f, 20f);
                pickedSpot = false;
            }
            else
            {
                if (indicatorController != null)
                {
                    indicatorResponse = indicatorController.LockLocation();
                    if (!indicatorResponse.isNull)
                    {
                        spawningLocation = indicatorResponse.centerOfAoe;
                        pickedSpot = true;
                        Invoke(nameof(CancelSpell), indicatorController.indicatorDeleteTimer);
                    }
                }
            }
        }
    }

    private void Fire()
    {
        if (firing)
        {
            for (int i=1; i <= projectilesPerSecond; i++)
            {
                Vector2 rad = (Random.insideUnitCircle - Vector2.one * 0.5f) * spawningRadius * 2;
                Vector3 spawn = spellLocation;
                spawn[0] += rad[0];
                spawn[2] += rad[1];
                
                Destroy(Instantiate(meteorPrefab, spawn, Quaternion.identity), 5f);
            }
            Invoke(nameof(Fire), 1f);
        }
    }

    private void StopFiring()
    {
        Clear();
        firing = false;
    }

    private void CancelSpell()
    {
        if (!firing)
        {
            Clear();
            pickedSpot = false;
        }
    }
    private void Clear()
    {
        indicatorController.DestroyIndicator();
        Destroy(tmpIndicatorHolder.gameObject);
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Fire;
    }
}
