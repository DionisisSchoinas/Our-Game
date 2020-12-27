﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
    private PlayerMovementScriptWarrior controls;
    private CharacterController characterController;
    private AnimationScriptControllerWarrior animations;
    private Transform transform;
    public AttackIndicator indicator;
    public float attackDelay;
    [HideInInspector]
    public bool attacking = false;//check if already attacking
    public bool canAttack = true;//check if the cooldown has passed
    public float meleeCooldown = 0.2f;
    bool canHit;
    //combo counters 
    public float reset;
    public float resetTime;
    //combo spacers 
    public float comboCurrent;
    public float comboTime=0.7f;
    //Combo queue 
    public List<int> comboQueue = new List<int>();
    //combo spam regulation
    bool comboLock;
    public float comboCoolDown;
    //direction lock
    public bool isDuringAttack;
    // Start is called before the first frame update
    void Start()
    {
        canHit = true;
        transform= GetComponent<Transform>() as Transform;
        indicator = GetComponent<AttackIndicator>() as AttackIndicator;
        controls = GetComponent<PlayerMovementScriptWarrior>() as PlayerMovementScriptWarrior;
        animations = GetComponent<AnimationScriptControllerWarrior>() as AnimationScriptControllerWarrior;
        isDuringAttack = false;
    }

    void FixedUpdate()
    {

        if (controls.mousePressed_1)
        {
            if ( canHit && !comboLock)
            {
                if (comboQueue.Count < 3)
                {

                    animations.Attack();
                    comboQueue.Add(0);
                    reset = 0f;
                     
                }
                canHit = false;
            }
        }
        else 
        {
            canHit = true;
        }

        if (comboQueue.Count != 0 && !attacking)
        {
            attacking = true;
            isDuringAttack = true;
            StartCoroutine(controls.stun(0.5f));
            StartCoroutine(PerformAttack(attackDelay));
            comboCurrent = 0f;

        }
        // else if (comboQueue.Count != 0)
        //{
        //    reset += Time.deltaTime;
        //    if (reset > resetTime)
        //    {
        //        attacking = false;
        //        //comboQueue.Clear();
        //
        //        animations.ResetAttack();
        //    }
        //}

        if (comboQueue.Count == 0)
        {
            isDuringAttack = false;
            animations.ResetAttack();
        }
        else if (comboQueue.Count == 3)
        {
            StartCoroutine(ComboCooldown(comboCoolDown));
           
        }
       
        if (attacking)
        {
           
            comboCurrent += Time.deltaTime;
       
            
            if (comboCurrent > comboTime)
            {
               
                comboQueue.RemoveAt(0);
                attacking = false;
            }
        }
    

    }
    IEnumerator PerformAttack(float attackDelay)
    {
       
        animations.Attack();
        yield return new WaitForSeconds(attackDelay);
        controls.sliding = true;
        
       
        foreach (Transform visibleTarget in indicator.visibleTargets)
        {
            Debug.Log(visibleTarget.name);
            HealthEventSystem.current.TakeDamage(visibleTarget.name, 30,0);
            HealthEventSystem.current.ApplyForce(visibleTarget.name,transform.forward, 5f);
        }
        yield return new WaitForSeconds(0.1f);
        controls.sliding = false;
    }
    IEnumerator ComboCooldown(float comboCooldown)
    {
        comboLock = true;
        yield return new WaitForSeconds(comboCooldown);
        comboLock = false;
    }
}