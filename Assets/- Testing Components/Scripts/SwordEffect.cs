using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SwordEffectAttributes
{
    public SwingTrailRenderer[] trails;
}

public class SwordEffect : MonoBehaviour
{
    public SwordEffectAttributes attributes;

    private SwordEffect currentEffect;
    private Transform tipPoint, basePoint;

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
            t.SetPoints(tipPoint, basePoint);
        }
    }

    public void StartSwing()
    {
        foreach (SwingTrailRenderer t in attributes.trails)
        {
            t.StartLine();
        }
    }

    public void StopSwing()
    {
        foreach (SwingTrailRenderer t in attributes.trails)
        {
            t.StopLine();
        }
    }
}
