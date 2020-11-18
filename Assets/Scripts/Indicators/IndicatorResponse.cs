using UnityEngine;

public class IndicatorResponse
{
    public bool isNull;
    public Vector3 centerOfAoe;
    public Vector3 spellRotation;
    public int face;

    public IndicatorResponse()
    {
        isNull = false;
    }

    public IndicatorResponse IsNull(bool state)
    {
        isNull = state;
        return this;
    }

    public IndicatorResponse CenterOfAoe(Vector3 cent)
    {
        centerOfAoe = cent;
        return this;
    }

    public IndicatorResponse SpellRotation(Vector3 rot)
    {
        spellRotation = rot;
        return this;
    }

    public IndicatorResponse Face(int f)
    {
        face = f;
        return this;
    }
}
