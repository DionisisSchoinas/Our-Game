using UnityEngine;

public class IndicatorResponse
{
    private bool isNull;
    private Vector3 centerOfAoe;
    private Vector3 spellRotation;
    private int face;

    public IndicatorResponse()
    {
        isNull = false;
    }

    public bool IsNull()
    {
        return isNull;
    }
    public IndicatorResponse IsNull(bool state)
    {
        isNull = state;
        return this;
    }

    public Vector3 CenterOfAoe()
    {
        return centerOfAoe;
    }
    public IndicatorResponse CenterOfAoe(Vector3 cent)
    {
        centerOfAoe = cent;
        return this;
    }

    public Vector3 SpellRotation()
    {
        return spellRotation;
    }
    public IndicatorResponse SpellRotation(Vector3 rot)
    {
        spellRotation = rot;
        return this;
    }

    public int Face()
    {
        return face;
    }
    public IndicatorResponse Face(int f)
    {
        face = f;
        return this;
    }
}
