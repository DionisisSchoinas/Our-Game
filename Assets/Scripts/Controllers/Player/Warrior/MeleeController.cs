using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
    public ParticleSystem readyForComboParticles;
    private ParticleSystem comboParticles;

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
    private bool canComboHit;
    // Counters for swings and limit
    private int comboSwings; // Counts the swings at their end
    private int clicks;  // Counts the number of swing commands when the user gives them
    // Direction lock
    public bool isDuringAttack;
    // Mouse lock
    private float lockedMouseClickTime;

    private bool skillListUp;
    private float lastCooldownDisplayMessage;
    private float lastManaDisplayMessage;
    public static float skillComboCooldown;

    private float currentMana;

    // Start is called before the first frame update
    void Start()
    {
        indicator = GetComponent<AttackIndicator>() as AttackIndicator;
        controls = GetComponent<PlayerMovementScriptWarrior>() as PlayerMovementScriptWarrior;
        animations = GetComponent<AnimationScriptControllerWarrior>() as AnimationScriptControllerWarrior;
        sword = GetComponent<Sword>() as Sword;

        comboParticles = Instantiate(readyForComboParticles, transform);

        comboQueue = new List<int>();

        attacking = false;
        canHit = true;
        comboLock = false;
        isDuringAttack = false;
        //lockedMouseClick = false;
        lockedMouseClickTime = 0f;

        comboCurrent = 0f;
        comboSwings = 0;
        clicks = 0;
        lastCooldownDisplayMessage = Time.time;
        lastManaDisplayMessage = Time.time;

        skillComboCooldown = comboCooldown;

        canComboHit = false;

        skillListUp = false;
        UIEventSystem.current.onSkillListUp += SkillListUp;
        ManaEventSystem.current.onManaUpdated += ManaUpdate;
    }

    private void OnDestroy()
    {
        UIEventSystem.current.onSkillListUp -= SkillListUp;
        ManaEventSystem.current.onManaUpdated -= ManaUpdate;
    }

    private void ManaUpdate(float mana)
    {
        currentMana = mana;
    }

    void FixedUpdate()
    {
        // Cooldown display message
        if (sword.GetSelectedEffect().onCooldown && controls.mousePressed_1)
        {
            if (Time.time - lastCooldownDisplayMessage >= 1f)
            {
                Debug.Log("On Cooldown");
                lastCooldownDisplayMessage = Time.time;
            }
            return;
        }

        // Mana display message
        if (currentMana < sword.GetSelectedEffect().manaCost && controls.mousePressed_1)
        {
            if (Time.time - lastManaDisplayMessage >= 1f)
            {
                Debug.Log("Not enough mana");
                lastManaDisplayMessage = Time.time;
            }
            return;
        }



        /*  Detect mouse click and start animation
         *
         *  Locks :
         *  
         *      skillListUp -----> list of skills is showing
         *      
         *      clicks < sword.GetSelectedEffect().comboPhaseMax -----> if the clicks are lower than the max allowed combo amount ( with a max of 2 you can do up to 2 combo hits )
         *      
         *      controls.allowHitAfterRoll -----> check if mid roll
         *      
         *      !sword.isCastinSkill -----> checks for sword being occupied
         *      
         *      sword.GetSelectedEffect().manaCost <= currentMana -----> checks if there is enough mana to cast the skill
         */
        if (lockedMouseClickTime < 0.1f)
        {
            lockedMouseClickTime += Time.deltaTime;
        }
        else if (controls.mousePressed_1 && !skillListUp && clicks < sword.GetSelectedEffect().comboPhaseMax && controls.allowHitAfterRoll && !sword.isCastingSkill)
        {
            if (canHit && !comboLock)
            {
                // Lock hits
                canHit = false;
                canComboHit = false;

                if (comboQueue.Count < 3)
                {
                    // Limit clicks to +2 of current swing
                    if (clicks <= comboSwings + 1)
                        clicks++;

                    if (!comboQueue.Contains(clicks))
                    {
                        lockedMouseClickTime = 0f;

                        // Animate with swing limit (stops a fake combo 3 from firing)
                        AttackAnimations(clicks);
                        comboQueue.Add(clicks);

                        lastCooldownDisplayMessage = Time.time;
                        lastManaDisplayMessage = Time.time;
                    }
                }
            }
        }

        // Start the actual attack function
        if (comboQueue.Count != 0 && !attacking && clicks <= sword.GetSelectedEffect().comboPhaseMax && comboSwings < sword.GetSelectedEffect().comboPhaseMax && !sword.isCastingSkill)
        {
            attacking = true;
            isDuringAttack = true;

            StartCoroutine(controls.Stun(sword.GetSelectedEffect().swingCooldowns[comboSwings] * 0.8f));

            Attack();

            comboCurrent = 0f;
            canComboHit = true;

            comboParticles.Stop();
        }
        else if (isDuringAttack && ( comboQueue.Count > 3 || comboSwings >= clicks || clicks > sword.GetSelectedEffect().comboPhaseMax || comboSwings >= sword.GetSelectedEffect().comboPhaseMax || (!attacking && comboQueue.Count != 0)) )
        {
            StartCooldowns();
            comboQueue.Clear();

            attacking = false;
            canHit = true;

            comboSwings = 0;
            clicks = 0;

            comboParticles.Stop();
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

            comboParticles.Stop();
        }
       
        // While the attack animation is playing
        if (attacking)
        {
            // Add time each frame
            comboCurrent += Time.deltaTime;
       
            // Check if the swing has ended
            if (comboCurrent > sword.GetSelectedEffect().swingCooldowns[sword.GetSelectedEffect().comboPhase])
            {
                comboSwings++;
                comboQueue.RemoveAt(0);
                attacking = false;
            }
            // Unlock input earlier
            else if (comboCurrent > sword.GetSelectedEffect().swingCooldowns[sword.GetSelectedEffect().comboPhase] * 0.5f && !canHit && canComboHit)
            {
                canHit = true;
                if (comboSwings + 1 < sword.GetSelectedEffect().comboPhaseMax)
                {
                    comboParticles.Play();
                    CameraShake.current.ShakeCamera(0.1f, 0.1f);
                }
            }
        }
    }

    private void AttackAnimations(int limit)
    {
        animations.Attack(limit);

        Debug.Log("animations : " + Time.time);
    }

    private void Attack()
    {
        sword.Attack(indicator, comboSwings);

        Debug.Log("damage : " + Time.time);
    }

    private void SkillListUp(bool up)
    {
        skillListUp = up;
    }
    /*
    IEnumerator LockMouse(float duration)
    {
        Debug.Log("lock");
        yield return new WaitForSeconds(duration);
        lockedMouseClick = false;
    }
    */
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
