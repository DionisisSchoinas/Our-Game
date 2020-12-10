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
        //trail.Stop();
    }

    private void Update()
    {
        if (allow && Input.GetKeyDown(KeyCode.Space))
        {
            swing = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            swing = false;
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

        //if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(StartSwing());
    }

    IEnumerator StartSwing()
    {
        animator.SetBool("Swing", true);
        yield return new WaitForSeconds(charge);
        //trail.Play();
        StartCoroutine(StopSwing());
    }

    IEnumerator StopSwing()
    {
        yield return new WaitForSeconds(swinging);
        //trail.Stop();
        yield return new WaitForSeconds(reset);
        animator.SetBool("Swing", false);
        allow = true;
    }
}
