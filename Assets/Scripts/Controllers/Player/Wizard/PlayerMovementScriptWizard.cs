using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class PlayerMovementScriptWizard : PlayerMovementScript
{
    public GameObject dodgeSkill;
    private WizardDodge dodgeScript;
    public float dodgeDistance = 10f;
    public float stunAfterDodge = 0.05f;

    private bool dodge;
    private bool dodging;
    private float lastDodge;
    private Vector3 dodgeDirection;
    private ParticleSystem dodgeParticleSystem;
    private CameraShake cameraShake;

    public new void Start()
    {
        base.Start();

        dodge = false;
        dodging = false;
        lastDodge = Time.time;

        dodgeParticleSystem = Instantiate(dodgeSkill).GetComponent<ParticleSystem>();
        dodgeParticleSystem.Stop();
        dodgeParticleSystem.transform.localScale = Vector3.one;
        dodgeScript = dodgeParticleSystem.gameObject.GetComponent<WizardDodge>();

        cameraShake = FindObjectOfType<CameraShake>();
    }

    public new void Update()
    {
        base.Update();

        if (!dodging && !dodgeScript.onCooldown)
            dodge = Input.GetKey(KeyCode.Space);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //=================== GROUND CHECK =================== 
        //check if its close to the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        //Reset the velocity 
        if (isGrounded && velocity.y < 0)
        {
            if (velocity.y <= -20)
            {
                StartCoroutine(Stun(2f));
            }
            velocity.y = -2f;
        }
        //=================== Dodge =================== 
        if (dodge && lastDodge + dodgeScript.cooldown <= Time.time)
        {
            dodgeDirection = Quaternion.Euler(0, 45, 0) * new Vector3(horizontal, 0f, vertical).normalized;
            if (dodgeDirection == Vector3.zero)
                dodgeDirection = transform.forward;
            dodgeParticleSystem.transform.position = controller.transform.position;
            dodgeParticleSystem.transform.rotation = Quaternion.LookRotation(dodgeDirection);

            HealthEventSystem.current.SetInvunerable(gameObject.name, true);
            StartCoroutine(DodgeTimer(dodgeScript.duration));
        }

        if (dodging)
        {
            controller.Move(dodgeDirection * (dodgeDistance / dodgeScript.duration) * Time.deltaTime);
            dodgeParticleSystem.transform.position = controller.transform.position;
        }


        //=================== Movement =================== 
        if (canMove)
        {
            //calculate and normalize direction
            direction = Quaternion.Euler(0, 45, 0) * new Vector3(horizontal, 0f, vertical).normalized;
          
        }
        else
        {
            direction = new Vector3(0f, 0f, 0f);
        }


        if (mousedown_1 || Wand.castingBasic)  // if mouse down OR if already firing basic
        {
            casting = true;
            if (canMove)
            {
                transform.rotation = indicatorWheel.rotation;
            }
        }
        else if (mousedown_2)  // if mouse down OR if already channeling
        {
            casting = true;
            if (canMove)
            {
                transform.rotation = indicatorWheel.rotation;
            }
        }
        else
        {
            casting = false;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothVelocity, smoothing);
            if (direction != new Vector3(0, 0, 0)) { transform.rotation = Quaternion.Euler(0f, angle, 0f); }
        }
        
        //Handles Running
        if (running)
        { 
            if (runspeed < maxRunSpeed - speed)
            {
                runspeed += 5f * Time.deltaTime;
                if (runspeed > (maxRunSpeed - speed))
                {
                    runspeed = maxRunSpeed - speed;
                }
            }
        }
        else
        {
            if (runspeed > 0f)
            {
                runspeed -= 5f * Time.deltaTime;
                if (runspeed < 0)
                {
                    runspeed = 0;
                }
            }

        }

        //Move player towords direction
        controller.Move(direction * (speed+ runspeed) * Time.deltaTime);
        
        if (jump && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jump = false;
        }

        //apply gravity to velocity 
        velocity.y += gravity * Time.deltaTime;
        //apply velocity to player
        controller.Move(velocity * Time.deltaTime);
    }

    private void DodgeEffect(bool enable)
    {
        if (dodgeParticleSystem == null)
            return;

        if (enable)
        {
            dodgeParticleSystem.Play();
            cameraShake.Shake(dodgeScript.duration / 2f, 1f);
        }
        else
        {
            dodgeParticleSystem.Stop();
        }
    }

    IEnumerator DodgeTimer(float seconds)
    {
        // Disable controls and enable particles
        DodgeEffect(true);
        canMove = false;
        dodging = true;
        dodge = false;
        // Dodge duration
        yield return new WaitForSeconds(seconds);
        // Enable controls and disable particles
        DodgeEffect(false);
        dodging = false;
        canMove = true;
        lastDodge = Time.time;

        dodgeScript.StartCooldown();
        UIEventSystem.current.Dodged(dodgeScript.cooldown);
        HealthEventSystem.current.SetInvunerable(gameObject.name, false);

        StartCoroutine(Stun(stunAfterDodge));
    }
}
