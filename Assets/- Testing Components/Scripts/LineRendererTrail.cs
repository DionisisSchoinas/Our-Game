using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererTrail : MonoBehaviour
{
    public int numberOfPointsSaved = 7;
    public Transform tipPoint;
    public Transform basePoint;
    [Range(0,1)]
    public float lineTolerance = 0;
    public Gradient lineGradient;
    public AnimationCurve lineCurve;
    public List<Material> lineMaterials;

    private LineRenderer line; 
    private List<Vector3> points;
    private bool allow;

    void Awake()
    {
        points = new List<Vector3>();
        allow = false;
        // Instantiate line
        line = gameObject.AddComponent<LineRenderer>();
        // Lighting
        line.receiveShadows = false;
        line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        // Poistions
        line.positionCount = 0;
        // Color
        line.colorGradient = lineGradient;
        // Curve
        AnimationCurve newCurve = new AnimationCurve();
        float mag = (tipPoint.position - basePoint.position).magnitude;
        foreach (Keyframe key in lineCurve.keys)
        {
            newCurve.AddKey(key.time, key.value * mag);
        }
        line.widthCurve = newCurve;
        // Materials
        line.materials = lineMaterials.ToArray();
    }

    void FixedUpdate()
    {
        if (allow)
        {
            if (points.Count == numberOfPointsSaved) points.RemoveAt(numberOfPointsSaved - 1);
            points.Insert(0, SpawnPoint());

            line.positionCount = points.Count;
            line.SetPositions(points.ToArray());
        }
    }

    public void StopLine()
    {
        allow = false;
        line.positionCount = 0;
        points.Clear();
    }

    public void StartLine()
    {
        allow = true;
    }

    private Vector3 SpawnPoint()
    {
        return Vector3.Lerp(basePoint.position, tipPoint.position, 0.5f);
    }
}
