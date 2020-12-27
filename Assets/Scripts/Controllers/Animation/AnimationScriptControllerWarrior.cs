﻿using System.Collections;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class AnimationScriptControllerWarrior : MonoBehaviour
{




    public Animator animator;
    public CharacterController player;
    public Transform indicatorWheel;
    public float velocityZ, velocityX;


    private PlayerMovementScriptWarrior controls;

    List<string> combos = new List<string>(new string[] { "Combo1", "Combo2", "Combo3" });
    public int combonum=0;
  
    // Start is called before the first frame update
    void Start()
    {
       
        controls = GameObject.FindObjectOfType<PlayerMovementScriptWarrior>() as PlayerMovementScriptWarrior;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //===== Movement Animations ======
        Vector3 direction = controls.direction;

        if (controls.isGrounded)
        {
            animator.SetBool("IsGrounded", true);
        }
        else
        {
            animator.SetBool("IsGrounded", false);
        }

        direction = Quaternion.Euler(0, -indicatorWheel.eulerAngles.y, 0) * direction;
        float velocityRun = (controls.runspeed) / (controls.maxRunSpeed - controls.speed);
        velocityZ = (direction.z + velocityRun * direction.z);
        velocityX = (direction.x + velocityRun * direction.x);

        if (!controls.canMove)
        {
            animator.SetTrigger("HardLanding");
        }

        //===== Spell Casting Animations ======
        if (!controls.casting && !controls.rolling)
        {
            animator.SetBool("Rolling", false);
            //fireboltHand.SetActive(false);
            animator.SetLayerWeight(1, 0);
            if (direction != new Vector3(0f, 0f, 0f))
            {
                animator.SetFloat("Velocity X", 0f);
                animator.SetFloat("Velocity Z", 1 + velocityRun);
            }
            else
            {
                animator.SetFloat("Velocity Z", 0f);
                animator.SetFloat("Velocity X", 0f);
            }
        }
        else if (controls.casting)
        {
        
       
            animator.SetFloat("Velocity Z", velocityZ);
            animator.SetFloat("Velocity X", velocityX);
        }
        else if (controls.rolling)
        {
            animator.SetBool("Rolling", true);

            
            animator.SetFloat("Velocity Z", velocityZ);
            animator.SetFloat("Velocity X", velocityX);
        }

        
    }

    public void Attack()
    {
       
       
        if (combonum < 3)
        {
         
            animator.SetBool(combos[combonum],true);
            combonum++;
          
        }
       
       



    }

    public void ResetAttack()
    {
        for (int cnum = 0; cnum < 3; cnum++)
        {

            animator.SetBool(combos[cnum], false);
        }
        combonum = 0;
    }

}
