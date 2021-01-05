using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class PlayerMovementScriptWarrior : PlayerMovementScript
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
    public float rollDistance = 10f;
    public float smoothing = 0.1f;
    float smoothVelocity;
    public float runspeed = 0f;
  
    public Vector3 direction;
   
    Vector3 velocity;

    public bool isGrounded;
    public bool canMove;
    public bool casting;

    private float horizontal;
    private float vertical;
    private bool running;
    private bool jump;

    //Dodge
    private bool roll;
    public bool rolling;
    private float rollTime = 0.9f;
    public float rollSpeed=15f;
    private Vector3 rollDirection;
    private float rollCooldown;
    private bool canRoll;
    //Slide(When attacking)
    public bool sliding;
 
    private float slidingSpeed = 10f;

    //----------------
    public GameObject particlesOnHit;
    //temp
    private MeleeController meleeController;
    

    private void Start()
    {
        meleeController = GameObject.FindObjectOfType<MeleeController>() as MeleeController;
        canMove = true;
        casting = false;
        mousedown_1 = false;
        mousedown_2 = false;
        mousePressed_1 = false;
        menu = false;
   
        horizontal = 0f;
        vertical = 0f;
        running = false;
        jump = false;

        //Roll
        canRoll = true ;

        lockMouseInputs = false;
    }

    private void Update()
    {
        //get horizontal and vertical axes
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

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
        if (Input.GetMouseButton(0) && !lockMouseInputs)
        {
            mousePressed_1 = true;
        }
        else
        {
            mousePressed_1 = false;
        }


        running = Input.GetKey(KeyCode.LeftShift);
        roll = Input.GetKey(KeyCode.Space);
        jump = Input.GetKey(KeyCode.L);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //===================GROUND CHECK=================== 
        //check if its close to the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        //Reset the velocity 
        if (isGrounded && velocity.y < 0)
        {
            if (velocity.y <= -20)
            {
                StartCoroutine(stun(2f));
            }
            velocity.y = -2f;
        }

        //===================Movement=================== 
        if (!rolling)
        {
            direction = (Quaternion.Euler(0, 45, 0) * new Vector3(horizontal, 0f, vertical).normalized);
        }

       
        if ( meleeController.isDuringAttack)  // if mouse down OR if already firing basic
        {
            transform.rotation = indicatorWheel.rotation;
        }
        else
        {
         
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
        if (roll && !rolling && canMove)
        {
           
            StartCoroutine(PerformRoll(rollTime));
           
        }

        

        if (rolling|| sliding)
        {
            controller.Move(transform.forward * rollSpeed * Time.deltaTime);
        }
        else
        {
            if (canMove) controller.Move(direction * (speed + runspeed) * Time.deltaTime);
        }
           
       
            
       
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

    public IEnumerator stun(float second)
    {
        canMove = false;
        yield return new WaitForSeconds(second);
        canMove = true;
    }
    private IEnumerator PerformRoll(float second)
    {
        rolling = true;
        yield return new WaitForSeconds(second);
        rolling = false;
    }
   
    private IEnumerator ResetCooldown(float second)
    {
        canRoll = false;
        yield return new WaitForSeconds(second);
        canRoll = true;
    }
    //public void getHit()
    //{
    //    FindObjectOfType<HitStop>().Stop(0.05f);
    //    FindObjectOfType<CameraShake>().Shake(0.05f);
    //    Destroy(Instantiate(particlesOnHit, transform.position + new Vector3(0, 1f, 0), transform.rotation), 1f);
    //    
    //}

}
