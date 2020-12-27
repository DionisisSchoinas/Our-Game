using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SwingTrailAttributes
{
    public float trailLengthInSeconds;
    public float minimumDistancePerVertex;
    public float delayAfterStartFrame;
    public Transform tipPoint;
    public Transform basePoint;
    public Gradient lineGradient;
    public AnimationCurve lineCurve;
    public List<Material> lineMaterials;

}

public class SwingTrailRenderer : MonoBehaviour
{
    public SwingTrailAttributes attributes;

    private TrailRenderer trail;
    private GameObject trailHolder;

    void Start()
    {
        // Instantiate Container
        trailHolder = new GameObject();
        trailHolder.transform.position = SpawnPoint();
        trailHolder.transform.SetParent(transform);
        // Instantiate Trail
        trail = trailHolder.AddComponent<TrailRenderer>();
        trail.alignment = LineAlignment.TransformZ;
        // Lighting
        trail.receiveShadows = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        // Length
        trail.time = attributes.trailLengthInSeconds;
        // Line Quality
        trail.minVertexDistance = attributes.minimumDistancePerVertex;
        // Color
        trail.colorGradient = attributes.lineGradient;
        // Curve
        AnimationCurve newCurve = new AnimationCurve();
        float mag = (attributes.tipPoint.position - attributes.basePoint.position).magnitude;
        foreach (Keyframe key in attributes.lineCurve.keys)
        {
            newCurve.AddKey(key.time, key.value * mag);
        }
        trail.widthCurve = newCurve;
        // Materials
        trail.materials = attributes.lineMaterials.ToArray();

        StopLine();
    }

    public void SetPoints(Transform tPoint, Transform bPoint)
    {
        attributes.tipPoint = tPoint;
        attributes.basePoint = bPoint;
    }

    void FixedUpdate()
    {
        if (trail.emitting)
        {
            trail.time = attributes.trailLengthInSeconds;
        }
    }

    public void StopLine()
    {
        trail.emitting = false;
        trail.enabled = false;
    }

    public void StartLine()
    {
        StartCoroutine(DelayedStart());
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(attributes.delayAfterStartFrame);
        trail.Clear();
        trail.emitting = true;
        trail.enabled = true;
    }

    private Vector3 SpawnPoint()
    {
        return Vector3.Lerp(attributes.basePoint.position, attributes.tipPoint.position, 0.5f);
    }

    private void OnDestroy()
    {
        Destroy(trailHolder.gameObject);
    }
}
