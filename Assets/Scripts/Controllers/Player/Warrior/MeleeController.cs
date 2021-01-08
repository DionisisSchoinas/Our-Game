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
    private int comboSwings; // Counts the swings at their end
    private int clicks;  // Counts the number of swing commands when the user gives them
    // Direction lock
    public bool isDuringAttack;
    // Mouse lock
    private bool lockedMouseClick;

    private bool skillListUp;
    private float lastCooldownDisplayMessage;
    public static float skillComboCooldown;

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
        lastCooldownDisplayMessage = Time.time;

        skillComboCooldown = comboCooldown;

        skillListUp = false;
        UIEventSystem.current.onSkillListUp += SkillListUp;
    }

    private void OnDestroy()
    {
        UIEventSystem.current.onSkillListUp -= SkillListUp;
    }

    void FixedUpdate()
    {
        /*  Detect mouse click and start animation
         *
         *  Locks :
         *      lockedMouseClick -----> locked by coroutine to stop spamming
         *      
         *      skillListUp -----> list of skills is showing
         *      
         *      sword.GetSelectedEffect().onCooldown -----> the selected skill is on cooldown
         *      
         *      clicks < sword.GetSelectedEffect().comboPhaseMax -----> if the clicks are lower than the max allowed combo amount ( with a max of 2 you can do up to 2 combo hits )
         *      
         *      controls.allowHitAfterRoll -----> check if mid roll
         */
        if (controls.mousePressed_1 && !lockedMouseClick && !skillListUp && !sword.GetSelectedEffect().onCooldown && clicks < sword.GetSelectedEffect().comboPhaseMax && controls.allowHitAfterRoll)
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

                    lastCooldownDisplayMessage = Time.time;
                }
            }
        }
        else if (!controls.mousePressed_1) // Lock mouse to avoid spam going thorough the canHit lock
        {
            StartCoroutine(LockMouse(0.05f));
        }

        // Cooldown display message
        if (sword.GetSelectedEffect().onCooldown && controls.mousePressed_1)
        {
            if (Time.time - lastCooldownDisplayMessage >= 1f)
            {
                Debug.Log("On Cooldown");
                lastCooldownDisplayMessage = Time.time;
            }
        }

        // Start the actual attack function
        if (comboQueue.Count != 0 && !attacking && comboSwings < clicks && clicks <= 3 && comboSwings < sword.GetSelectedEffect().comboPhaseMax)
        {
            attacking = true;
            isDuringAttack = true;

            StartCoroutine(controls.Stun(0.5f));

            Attack();

            comboCurrent = 0f;
        }
        else if (isDuringAttack && ( comboSwings >= 3 || comboQueue.Count > 3 || comboSwings >= clicks || clicks > 3 || comboSwings >= sword.GetSelectedEffect().comboPhaseMax) )
        {
            StartCooldowns();
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
            else if (comboCurrent > sword.GetSelectedEffect().swingCooldown * 0.52f && !canHit)
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

    private void SkillListUp(bool up)
    {
        skillListUp = up;
    }

    IEnumerator LockMouse(float duration)
    {
        lockedMouseClick = true;
        yield return new WaitForSeconds(duration);
        lockedMouseClick = false;
    }

    private void StartCooldowns()
    {
        sword.GetSelectedEffect().StartCooldown();
        StartCoroutine(ComboCooldown(comboCooldown));
    }

    IEnumerator ComboCooldown(float comboCooldown)
    {
        comboLock = true;
        yield return new WaitForSeconds(comboCooldown * 0.5f);
        sword.isSwinging = false;
        yield return new WaitForSeconds(comboCooldown * 0.5f);
        comboLock = false;
    }
}
