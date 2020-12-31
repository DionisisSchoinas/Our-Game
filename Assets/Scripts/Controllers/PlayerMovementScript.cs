using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public CharacterController controller;
    public Transform indicatorWheel;
    public Transform groundCheck;
    public Transform Cylinder;
    public LayerMask groundMask;
    
    public float speed = 6f;
    public float maxRunSpeed = 12f;
    public float gravity = -9.81f;
    public float groundDistance = 0.4f;
    public float jumpHeight = 2f;
    public float smoothing = 0.1f;
    float smoothVelocity;
    public float runspeed = 0f;

    public ParticleSystem dodgeParticles;
    public float dodgeDuration = 0.5f;
    public float dodgeDistance = 10f;
    public float dodgeCooldown = 0.5f;
    public float stunAfterDodge = 0.05f;

    public Vector3 direction;
   
    Vector3 velocity;

    public bool isGrounded;
    public bool canMove;
    public bool casting;
    // Controlled inputs which lock other if one is pressed
    public bool mousedown_1;
    public bool mousedown_2;
    // ---------------
    public bool menu;
    // Raw inputs
    public bool mouse_1;
    public bool mouse_2;
    // ---------------
    public bool lockMouseInputs;

    private float horizontal;
    private float vertical;
    private bool running;
    private bool jump;
    private bool dodge;
    private bool dodging;
    private float lastDodge;
    private Vector3 dodgeDirection;
    private ParticleSystem dodgeParticleSystem;
    private CameraShake cameraShake;

    private void Start()
    {
        canMove = true;
        casting = false;
        mousedown_1 = false;
        mousedown_2 = false;
        menu = false;
        lockMouseInputs = false;

        horizontal = 0f;
        vertical = 0f;
        running = false;
        jump = false;
        dodge = false;
        dodging = false;
        lastDodge = Time.time;

        dodgeParticleSystem = Instantiate(dodgeParticles);
        dodgeParticleSystem.Stop();
        dodgeParticleSystem.transform.localScale = Vector3.one;

        cameraShake = FindObjectOfType<CameraShake>();
    }

    private void Update()
    {
        //get horizontal and vertical axes
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        //pciking spell
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            menu = true;
            mousedown_1 = false;
            mousedown_2 = false;
        }
        else if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            menu = false;
        }

        if (!menu)
        {
            // Controlled inputs which lock other if one is pressed
            if (Input.GetMouseButtonDown(0) && !mousedown_2 && !lockMouseInputs)
            {
                mousedown_1 = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                mousedown_1 = false;
            }
            if (Input.GetMouseButtonDown(1) && !mousedown_1 && !lockMouseInputs)
            {
                mousedown_2 = true;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                mousedown_2 = false;
            }

            // Raw inputs
            if (Input.GetMouseButtonDown(0) && !lockMouseInputs)
            {
                mouse_1 = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                mouse_1 = false;
            }
            if (Input.GetMouseButtonDown(1) && !lockMouseInputs)
            {
                mouse_2 = true;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                mouse_2 = false;
            }
        }

        running = Input.GetKey(KeyCode.LeftShift);
        jump = Input.GetKey(KeyCode.C);

        if (!dodging)
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
        if (dodge && lastDodge + dodgeCooldown <= Time.time)
        {
            dodgeDirection = Quaternion.Euler(0, 45, 0) * new Vector3(horizontal, 0f, vertical).normalized;
            if (dodgeDirection == Vector3.zero)
                dodgeDirection = transform.forward;
            dodgeParticleSystem.transform.position = controller.transform.position;
            dodgeParticleSystem.transform.rotation = Quaternion.LookRotation(dodgeDirection);

            StartCoroutine(DodgeTimer(dodgeDuration));
        }

        if (dodging)
        {
            controller.Move(dodgeDirection * (dodgeDistance / dodgeDuration) * Time.deltaTime);
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
        if (dodgeParticles == null)
            return;

        if (enable)
        {
            dodgeParticleSystem.Play();
            cameraShake.Shake(dodgeDuration / 2f, 1f);
        }
        else
        {
            dodgeParticleSystem.Stop();
        }
    }

    IEnumerator Stun(float second)
    {
        canMove = false;
        yield return new WaitForSeconds(second);
        canMove = true;
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
        StartCoroutine(Stun(stunAfterDodge));
    }
}
