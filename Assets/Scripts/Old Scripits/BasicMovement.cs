using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private Vector3 speedVector = new Vector3(-2f, 0f, 0f);

    private void FixedUpdate()
    {
        rb.AddForce(speedVector);
    }
}
