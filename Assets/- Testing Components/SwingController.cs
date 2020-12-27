using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingController : MonoBehaviour
{
    Animator animator;
    bool swing;
    
    public ParticleSystem trail;
    public float charge;
    public float swinging;
    public float reset;

    bool allow;

    Coroutine coroutine;

    void Start()
    {
        animator = GetComponent<Animator>();
        allow = true;
        swing = false;
    }

    private void Update()
    {
        if (allow && Input.GetKey(KeyCode.Space))
        {
            swing = true;
        }
    }

    private void FixedUpdate()
    {
        if (swing)
        {
            swing = false;
            Swing();
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            Fire();
        }
    }

    private void Fire()
    {

    }

    private void Swing()
    {
        allow = false;
        coroutine = StartCoroutine(StartSwing());
    }

    IEnumerator StartSwing()
    {
        animator.SetBool("Swing", true);
        yield return new WaitForSeconds(charge);
        StartCoroutine(StopSwing());
    }

    IEnumerator StopSwing()
    {
        yield return new WaitForSeconds(swinging);
        yield return new WaitForSeconds(reset);
        animator.SetBool("Swing", false);
        allow = true;
    }
}
