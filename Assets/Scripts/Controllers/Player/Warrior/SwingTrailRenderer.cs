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
        trailHolder.transform.SetParent(transform);
        trailHolder.transform.position = SpawnPoint();
        trailHolder.transform.rotation = transform.rotation;

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
        trail.widthCurve = attributes.lineCurve;
        trail.widthMultiplier = (attributes.tipPoint.position - attributes.basePoint.position).magnitude;
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
        trail.Clear();
        yield return new WaitForSeconds(attributes.delayAfterStartFrame);
        trail.enabled = true;
        trail.emitting = true;
    }

    private Vector3 SpawnPoint()
    {
        return Vector3.Lerp(attributes.basePoint.position, attributes.tipPoint.position, 0.5f);
    }
    /*
    private Vector3 LookAtPoint()
    {
        Vector3 center = SpawnPoint();
        Vector3 dir = center - attributes.basePoint.position;
        return Vector3.Cross(dir, Vector3.up).normalized + center;
    }
    */
    private void OnDestroy()
    {
        Destroy(trailHolder.gameObject);
    }
}
