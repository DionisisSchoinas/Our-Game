using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arc : MonoBehaviour
{
    Vector3 to;
    float timeSpawned, secondsAlive;
    int breakPoints;
    bool enabledSparks, withFrames;
    int changePerSecond;

    LineRenderer lineRenderer;

    private void Awake()
    {
        this.secondsAlive = 1f;

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 1f;
        lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lineRenderer.receiveShadows = false;
        lineRenderer.positionCount = 6;
        if (Random.value <= 0.5f) lineRenderer.material = ResourceManager.Materials.LightningArc1;
        else lineRenderer.material = ResourceManager.Materials.LightningArc2;

        enabledSparks = true;
        changePerSecond = -1;
        withFrames = true;

        gameObject.SetActive(false);
    }

    public void Enable()
    {
        if (!withFrames)
        {
            InvokeRepeating(nameof(SetPoints), 0f, 1f / changePerSecond);
        }
        timeSpawned = Time.time;
        gameObject.SetActive(true);
    }

    public Arc To(Vector3 to)
    {
        this.to = to;
        return this;
    }
    public Arc From(Vector3 from)
    {
        transform.position = from;
        return this;
    }
    public Arc SecondsAlive(float secondsAlive)
    {
        this.secondsAlive = secondsAlive;
        return this;
    }
    public Arc Width(float width)
    {
        lineRenderer.startWidth = width;
        return this;
    }
    public Arc BreakPoints(int breakPoints)
    {
        this.breakPoints = (breakPoints > 2) ? breakPoints : 2;
        lineRenderer.positionCount = this.breakPoints;
        return this;
    }
    public Arc EnabledSparks(bool enabledSparks)
    {
        this.enabledSparks = enabledSparks;
        return this;
    }
    public Arc ChangeTicksPerSecond(int ticks)
    {
        this.changePerSecond = (ticks > 0) ? ticks : -1;
        this.withFrames = (ticks == -1);
        return this;
    }
    public Arc Speed(Vector2 speed)
    {
        Material mat = lineRenderer.material;
        mat.SetVector("_Speed", speed);
        lineRenderer.material = mat;
        return this;
    }

    private void FixedUpdate()
    {
        if (withFrames) 
            SetPoints();
        if (Time.time >= timeSpawned + secondsAlive)
        {
            Destroy(gameObject);
        }
    }

    private void SetPoints()
    {
        Ray ray = new Ray(transform.position, (to - transform.position).normalized);
        float points = (to - transform.position).magnitude / breakPoints;
        lineRenderer.SetPosition(0, transform.position);
        for (int i=1; i < breakPoints-1; i++)
        {
            lineRenderer.SetPosition(i, ray.GetPoint(i * points) + Random.insideUnitSphere * 0.5f);
        }
        lineRenderer.SetPosition(breakPoints-1, to);

        if (enabledSparks)
            Destroy(Instantiate(ResourceManager.Effects.Sparks, to, Quaternion.identity).gameObject, 0.3f);
    }
}
