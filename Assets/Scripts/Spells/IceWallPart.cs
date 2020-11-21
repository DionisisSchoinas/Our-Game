using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceWallPart : MonoBehaviour
{
    private float height;
    private int steps;

    private Vector3 maxHeight;
    private Vector3 minHeight;
    private Vector3 maxPosition;
    private Vector3 minPosition;
    private float counter;

    private bool spawn;
    private bool melt;

    void Start()
    {
        height = 8f;
        steps = 30;

        maxHeight = transform.localScale;
        transform.localScale = transform.localScale + Vector3.down * height;
        minHeight = transform.localScale;

        maxPosition = transform.position;
        transform.position -= transform.up * height / 2f;
        minPosition = transform.position;

        counter = 0f;
        spawn = true;
        melt = false;
    }

    void FixedUpdate()
    {
        if (spawn) Rise();
        else if (melt) Melt();
    }
    
    private void Rise()
    {
        if (counter <= 1f)
        {
            transform.localScale = Vector3.Lerp(minHeight, maxHeight, counter);
            transform.position = Vector3.Lerp(minPosition, maxPosition, counter);
            counter += Random.Range(0.3f / steps, 1.5f / steps);
        }
        else
        {
            spawn = false;
            Invoke(nameof(StartMelting), 5f);
        }
    }

    private void StartMelting()
    {
        counter = 0;
        melt = true;
    }

    private void Melt()
    {
        if (counter <= 1f)
        {
            transform.localScale = Vector3.Lerp(minHeight, maxHeight, 1-counter);
            transform.position = Vector3.Lerp(minPosition, maxPosition, 1-counter);
            counter += Random.Range(0.1f / steps, 0.3f / steps);
        }
        else if (counter <= 2f)
        {
            counter += 0.05f;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
