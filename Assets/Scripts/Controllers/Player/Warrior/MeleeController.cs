using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
    // controllers
    private PlayerMovementScriptWarrior controls;
    private AnimationScriptControllerWarrior animations;
    private Sword sword;

    public AttackIndicator indicator;
    public float attackDelay;
    [HideInInspector]
    public bool attacking = false;//check if already attacking
    public bool canAttack = true;//check if the cooldown has passed
    //public float meleeCooldown = 0.2f;
    bool canHit;
    //combo counters 
    /*
    public float reset;
    public float resetTime;
    */
    //combo spacers 
    public float comboCurrent;
    public float comboTime = 0.7f;
    //Combo queue 
    public List<int> comboQueue = new List<int>();
    //combo spam regulation
    bool comboLock;
    public float comboCoolDown;
    //direction lock
    public bool isDuringAttack;

    private float lastSwingTime;


    // Start is called before the first frame update
    void Start()
    {
        lastSwingTime = Time.time;
        canHit = true;
        indicator = GetComponent<AttackIndicator>() as AttackIndicator;
        controls = GetComponent<PlayerMovementScriptWarrior>() as PlayerMovementScriptWarrior;
        animations = GetComponent<AnimationScriptControllerWarrior>() as AnimationScriptControllerWarrior;
        sword = GetComponent<Sword>() as Sword;
        isDuringAttack = false;
    }

    void FixedUpdate()
    {
        if (!sword.GetSelectedEffect().onCooldown && Time.time - lastSwingTime >= sword.GetSelectedEffect().swingCooldown)
        {
            if (controls.mousePressed_1)
            {
                if (canHit && !comboLock)
                {
                    if (comboQueue.Count < 3)
                    {

                        AttackAnimations();
                        comboQueue.Add(0);
                        //reset = 0f;

                    }
                    canHit = false;
                    lastSwingTime = Time.time;
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
                StartCoroutine(controls.Stun(0.5f));

                //StartCoroutine(PerformAttack(attackDelay));
                Attack();

                comboCurrent = 0f;

            }
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
        else if (comboQueue.Count >= 3)
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

    private void AttackAnimations()
    {
        sword.StartSwing();
        animations.Attack();
    }

    private void Attack()
    {
        AttackAnimations();
        sword.Attack(controls, indicator);
    }

    IEnumerator ComboCooldown(float comboCooldown)
    {
        comboLock = true;
        yield return new WaitForSeconds(comboCooldown);
        comboLock = false;
    }
}
