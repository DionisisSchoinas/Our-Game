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

    public override string type => "Sword Effect";
    public override string skillName => "Sword Effect";
    public override float cooldown { get => 0.7f; }
    public override float duration { get => 0f; }

    public new void Awake()
    {
        base.Awake();
        trails = new List<SwingTrailRenderer>();
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

    public void StartSwing()
    {
        foreach (SwingTrailRenderer t in trails)
        {
            t.StartLine();
        }
    }

    public void StopSwing()
    {
        foreach (SwingTrailRenderer t in trails)
        {
            t.StopLine();
        }
    }

    public override void Attack(PlayerMovementScriptWarrior controls, AttackIndicator indicator, SkinnedMeshRenderer playerMesh)
    {
    }
}
