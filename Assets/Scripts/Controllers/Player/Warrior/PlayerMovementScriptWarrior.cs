using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class PlayerMovementScriptWarrior : PlayerMovementScript
{
    public float rollDistance = 10f;

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
    

    public new void Start()
    {
        base.Start();

        meleeController = GameObject.FindObjectOfType<MeleeController>() as MeleeController;

        //Roll
        canRoll = true ;
    }

    public new void Update()
    {
        base.Update();

        roll = Input.GetKey(KeyCode.Space);
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
                StartCoroutine(Stun(2f));
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
