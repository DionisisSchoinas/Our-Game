using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeThroughController : MonoBehaviour
{
    public Vector3 cutOutSize = new Vector3(7,7,7);
    public Camera mainCamera;
    public float waitTimeForEachSize = 0.001f;
    public float startSizeOnEnlarge = 3f;

    private Coroutine coroutine;
    private float counter;
    private bool allowEnlarge;

    private void Awake()
    {
        coroutine = null;
        counter = 0;
        allowEnlarge = true;
    }

    void FixedUpdate()
    {
        Vector3 straightLine = transform.position - mainCamera.transform.position;
        Ray ray = new Ray(mainCamera.transform.position, straightLine.normalized);
        if (Physics.Raycast(ray, straightLine.magnitude, BasicLayerMasks.CuttableWalls))
        {
            if (allowEnlarge)
                StartCutting();
        }
        else if (counter != 0)
        {
            StopCutting();
        }
    }

    private void StartCutting()
    {
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(Enlarge());
    }
    private void StopCutting()
    {
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(Shrink());
    }

    private IEnumerator Enlarge()
    {
        allowEnlarge = false;
        while (counter < 1)
        {
            transform.localScale = Vector3.Lerp(Vector3.one * startSizeOnEnlarge, cutOutSize, counter);
            counter += 0.01f;
            yield return new WaitForSeconds(waitTimeForEachSize);
        }
        yield return null;
    }
    private IEnumerator Shrink()
    {
        allowEnlarge = true;
        while (counter > 0)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, cutOutSize, counter);
            counter -= 0.01f;
            yield return new WaitForSeconds(waitTimeForEachSize);
        }
        yield return null;
    }
}
