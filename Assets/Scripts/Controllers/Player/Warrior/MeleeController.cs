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

    private bool attacking; //check if already attacking
    private bool canHit;
    //Combo spacers 
    private float comboCurrent;
    //Combo queue 
    private List<int> comboQueue;
    // Combo spam regulation
    public bool comboLock;
    public float comboCooldown;
    // Counters for swings and limit
    private int comboSwings;
    private int clicks;
    // Direction lock
    public bool isDuringAttack;
    // Mosue lock
    private bool lockedMouseClick;

    // Start is called before the first frame update
    void Start()
    {
        indicator = GetComponent<AttackIndicator>() as AttackIndicator;
        controls = GetComponent<PlayerMovementScriptWarrior>() as PlayerMovementScriptWarrior;
        animations = GetComponent<AnimationScriptControllerWarrior>() as AnimationScriptControllerWarrior;
        sword = GetComponent<Sword>() as Sword;

        comboQueue = new List<int>();

        attacking = false;
        canHit = true;
        comboLock = false;
        isDuringAttack = false;
        lockedMouseClick = false;

        comboCurrent = 0f;
        comboSwings = 0;
        clicks = 0;
    }

    void FixedUpdate()
    {
        // Detect mouse click and start animation
        if (controls.mousePressed_1 && !lockedMouseClick)
        {
            if (canHit && !comboLock)
            {
                // Lock hits
                canHit = false;
                if (comboQueue.Count < 3)
                {
                    // Limit clicks to +2 of current swing
                    if (clicks <= comboSwings + 1)
                        clicks++;

                    // Animate with swing limit (stops a fake combo 3 from firing)
                    AttackAnimations(clicks);
                    comboQueue.Add(0);
                }
            }
        }
        else if (!controls.mousePressed_1) // Lock mouse to avoid spam going thorough the canHit lock
        {
            StartCoroutine(LockMouse(0.05f));
        }

        // Start the actual attack function
        if (comboQueue.Count != 0 && !attacking && comboSwings < clicks && clicks <= 3)
        {
            attacking = true;
            isDuringAttack = true;

            StartCoroutine(controls.Stun(0.5f));

            Attack();

            comboCurrent = 0f;
        }
        else if (isDuringAttack && ( comboSwings >= 3 || comboQueue.Count > 3 || comboSwings >= clicks || clicks > 3) )
        {
            StartCoroutine(ComboCooldown(comboCooldown));
            comboQueue.Clear();

            attacking = false;
            canHit = true;

            comboSwings = 0;
            clicks = 0;
        }


        if (comboQueue.Count == 0 && comboSwings != 0)
        {
            comboSwings = 0;
        }
        
        if (comboQueue.Count == 0 && clicks != 0)
        {
            clicks = 0;
        }
        
        if (comboQueue.Count == 0 && isDuringAttack)
        {
            isDuringAttack = false;
            animations.ResetAttack();
        }
       
        // While the attack animation is playing
        if (attacking)
        {
            // Add time each frame
            comboCurrent += Time.deltaTime;
       
            // Check if the swing has ended
            if (comboCurrent > sword.GetSelectedEffect().swingCooldown)
            {
                comboSwings++;
                comboQueue.RemoveAt(0);
                attacking = false;
            }
            // Unlock input earlier
            else if (comboCurrent > sword.GetSelectedEffect().swingCooldown * 0.6f && !canHit)
            {
                canHit = true;
            }
        }
    }

    private void AttackAnimations(int limit)
    {
        animations.Attack(limit);
    }

    private void Attack()
    {
        sword.Attack(controls, indicator, comboSwings);
    }


    IEnumerator LockMouse(float duration)
    {
        lockedMouseClick = true;
        yield return new WaitForSeconds(duration);
        lockedMouseClick = false;
    }

    IEnumerator ComboCooldown(float comboCooldown)
    {
        comboLock = true;
        yield return new WaitForSeconds(comboCooldown * 0.75f);
        sword.isSwinging = false;
        yield return new WaitForSeconds(comboCooldown * 0.25f);
        comboLock = false;
    }
}
