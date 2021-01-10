using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SwordEffectAttributes
{
    public SwingTrailRenderer[] trails;
    public Material swordMaterial;
}

public class SwordEffect : BasicSword
{
    public SwordEffectAttributes attributes;

    private List<SwingTrailRenderer> trails;
    private SwordEffect currentEffect;
    private Transform tipPoint, basePoint;
    private GameObject swordParticles;

    protected bool instaCasting;

    public override string type => "Sword Effect";
    public override string skillName => "Sword Effect";
    public override float cooldown => 0.7f;
    public override float duration => 0f;
    public override int comboPhaseMax => 3;
    public override float instaCastDelay => 0.4f;
    public override bool instaCast => false;
    public override float manaCost => 0f;

    public new void Awake()
    {
        base.Awake();
        trails = new List<SwingTrailRenderer>();
        instaCasting = false;
    }

    protected void OnDestroy()
    {
        if (swordParticles != null)
            Destroy(swordParticles);
    }

    public SwordEffect InstantiateEffect(Transform tPoint, Transform bPoint, Transform parent)
    {
        currentEffect = Instantiate(gameObject, parent).GetComponent<SwordEffect>();
        currentEffect.SetPoints(tPoint, bPoint);
        return currentEffect;
    }

    public void SetPoints(Transform tPoint, Transform bPoint)
    {
        tipPoint = tPoint;
        basePoint = bPoint;
        foreach (SwingTrailRenderer t in attributes.trails)
        {
            trails.Add(Instantiate(t, transform));
            trails[trails.Count - 1].SetPoints(tipPoint, basePoint);
        }
    }

    public void StartSwingTrail()
    {
        foreach (SwingTrailRenderer t in trails)
        {
            t.StartLine();
        }
    }

    public void StopSwingTrail()
    {
        foreach (SwingTrailRenderer t in trails)
        {
            t.StopLine();
        }
    }

    // This function instanitates a cipy of itself and calls the coroutine FROM THE COPY
    public SwordEffect InstaCast(PlayerMovementScriptWarrior controls, GameObject swordObject, SkinnedMeshRenderer playerMesh, Renderer swordRenderer, Transform swordTopPoint, Transform swordBasePoint, Transform swordParent)
    {
        SwordEffect current = InstantiateEffect(swordTopPoint, swordBasePoint, swordParent);
        current.StartCast(controls, swordObject, playerMesh, swordRenderer);

        return current;
    }

    // Called in the copy of the object
    public void StartCast(PlayerMovementScriptWarrior controls, GameObject swordObject, SkinnedMeshRenderer playerMesh, Renderer swordRenderer)
    {
        if (isActiveAndEnabled)
            StartCoroutine(DelayInstaCast(controls, swordObject, playerMesh, swordRenderer));
    }

    private IEnumerator DelayInstaCast(PlayerMovementScriptWarrior controls, GameObject swordObject, SkinnedMeshRenderer playerMesh, Renderer swordRenderer)
    {
        instaCasting = true;

        yield return new WaitForSeconds(instaCastDelay);

        if (swordParticles != null)
            Destroy(swordParticles);

        if (GetSource() != null)
        {
            swordParticles = Instantiate(GetSource().gameObject, swordObject.transform);
            swordParticles.transform.rotation = swordObject.transform.rotation;
        }

        swordRenderer.material = attributes.swordMaterial;

        if (instaCast)
        {
            Attack(controls, null, playerMesh);
            
            StartCooldown();
            UIEventSystem.current.FreezeAllSkills(uniqueOverlayToWeaponAdapterId, swingCooldown * 0.5f);
        }

        instaCasting = false;
    }

    public override void Attack(PlayerMovementScriptWarrior controls, AttackIndicator indicator, SkinnedMeshRenderer playerMesh)
    {
    }

    public override ParticleSystem GetSource()
    {
        return null;
    }
}
