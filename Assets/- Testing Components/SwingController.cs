using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingController : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            animator.SetBool("Swing", true);
        }
        else
        {
            animator.SetBool("Swing", false);
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            Fire();
        }
    }

    private void Fire()
    {

    }
}
