using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arc : MonoBehaviour
{
    Vector3 to;
    float timeSpawned, secondsAlive;
    int breakPoints;

    LineRenderer lineRenderer;

    private void Awake()
    {
        this.secondsAlive = 1f;
        timeSpawned = Time.time;

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 1f;
        lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lineRenderer.receiveShadows = false;
        lineRenderer.positionCount = 6;
        if (Random.value <= 0.5f) lineRenderer.material = ResourceManager.Materials.LightningArc1;
        else lineRenderer.material = ResourceManager.Materials.LightningArc2;

        gameObject.SetActive(false);
    }

    public void SetValues(Vector3 to)
    {
        this.to = to;
        gameObject.SetActive(true);
    }


    public void SetValues(Vector3 to, float secondsAlive)
    {
        this.secondsAlive = secondsAlive;
        SetValues(to);
    }

    public void SetValues(Vector3 to, float secondsAlive, float width)
    {
        lineRenderer.startWidth = width;
        SetValues(to, secondsAlive);
    }
    public void SetValues(Vector3 to, float secondsAlive, float width, int breakPoints)
    {
        this.breakPoints = (breakPoints > 2) ? breakPoints : 2;
        lineRenderer.positionCount = this.breakPoints;
        SetValues(to, secondsAlive, width);
    }

    private void FixedUpdate()
    {
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
    }


}
