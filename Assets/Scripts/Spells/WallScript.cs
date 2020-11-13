using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour
{
    private float width, height;

    private Vector3 minHeight, maxHeight;
    private Vector3 minPosition, maxPosition;
    private float counter;

    void Start()
    {
        maxHeight = transform.localScale;
        transform.localScale = Vector3.zero + Vector3.forward * 0.5f + Vector3.right * width;
        minHeight = transform.localScale;

        maxPosition = transform.position;
        transform.position += transform.up * height / 2f;
        minPosition = transform.position;

        counter = 0;
    }

    void FixedUpdate()
    {
        if (transform.localScale.y < height)
        {
            transform.localScale = Vector3.Lerp(minHeight, maxHeight, counter);
            transform.position = Vector3.Lerp(minPosition, maxPosition, counter);
            counter += 0.05f;
        }
    }

    public void SetScale(Vector3 scale)
    {
        transform.localScale = scale;
        width = transform.localScale.x;
        height = transform.localScale.y;
    }
}
