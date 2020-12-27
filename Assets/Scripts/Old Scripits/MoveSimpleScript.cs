using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSimpleScript : MonoBehaviour
{
    public Vector3 move;
    public bool forward;
    public float distanceToBounce;
    public float secondsPauseAtEdge;

    private Vector3 startLoc;
    private bool allow;

    private void Start()
    {
        forward = true;
        startLoc = transform.position;
        allow = true;
    }

    void FixedUpdate()
    {
        if (allow)
        {
            transform.position += move * (forward ? 1 : -1) * Time.deltaTime;
            if ((transform.position - startLoc).magnitude >= distanceToBounce)
            {
                forward = !forward;
                StartCoroutine(Wait());
            }
        }
    }

    IEnumerator Wait()
    {
        allow = false;
        yield return new WaitForSeconds(secondsPauseAtEdge);
        allow = true;
    }
}
