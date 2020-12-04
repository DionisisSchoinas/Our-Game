using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleViewerCamera : MonoBehaviour
{
    public Camera mainCam;


    // Start is called before the first frame update
    void Start()
    {
        mainCam = GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
        }
    }
}
