using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Wand : MonoBehaviour
{
    [SerializeField]
    private AnimationScriptController animationController;
    [SerializeField]
    private Transform simpleFirePoint;
    [SerializeField]
    private Transform channelingFirePoint;
    [SerializeField]
    private Spell defaultSpell;
    [SerializeField]
    private Spell[] spells;

    //=== Values must be equal with AnimationScriptControler ===
    public float castingAnimationSimple = 0.8f;
    public float castingAnimationSimpleReset = 1.4f;
    public float castingAnimationChannel = 1.3f;
    public float castingAnimationChannelReset = 1f;
    //=============
    public bool channeling;
    public static bool castingBasic;
    public bool canRelease;
    public bool casting;
    //=============

    private bool canCast;
    private int selectedSpell;
    private Spell currentSpell;
    private Coroutine runningCoroutine;
    private float lastCooldownDisplayMessage;
    private float lastManaDisplayMessage;

    private bool skillListUp;
    private float currentMana;

    private void Start()
    {
        casting = false;
        lastCooldownDisplayMessage = Time.time;
        lastManaDisplayMessage = Time.time; 

        castingBasic = false;
        channeling = false;
        canCast = true;
        canRelease = false;

        foreach (Spell s in spells)
        {
            s.WakeUp();
        }

        skillListUp = false;
        UIEventSystem.current.onSkillListUp += SkillListUp;
        ManaEventSystem.current.onManaUpdated += ManaUpdate;
    }

    private void OnDestroy()
    {
        UIEventSystem.current.onSkillListUp -= SkillListUp;
        ManaEventSystem.current.onManaUpdated -= ManaUpdate;
    }

    private void FixedUpdate()
    {
        // Stop Ray spells when they are firing and they drain all mana
        if (casting && currentSpell.type == "Ray" && currentMana < currentSpell.manaCost)
        {
            Fire2(false);
        }
    }

    private void ManaUpdate(float mana)
    {
        currentMana = mana;
    }

    private void SkillListUp(bool up)
    {
        skillListUp = up;
    }

    public Spell GetDefaultSpell()
    {
        return defaultSpell;
    }

    public List<Spell> GetSpells()
    {
        return spells.ToList();
    }

    public Spell GetSelectedSpell()
    {
        return currentSpell;
    }

    public bool SetSelectedSpell(int value)
    {
        // Release held spells
        if (castingBasic)
        {
            Cancel();
            StartCoroutine(ChangeSelectedIndex(castingAnimationSimple + castingAnimationSimpleReset / 2f, value));
            return true;
        }
        if (channeling)
        {
            Cancel();
        }
        SetSpellIndex(value);
        return true;
    }

    private void SetSpellIndex(int value)
    {
        selectedSpell = value;
        if (value == -1)
            currentSpell = defaultSpell;
        else
            currentSpell = spells[value];
    }

    IEnumerator ChangeSelectedIndex(float delay, int value)
    {
        yield return new WaitForSeconds(delay);
        SetSpellIndex(value);
    }

    public void Fire(bool holding)
    {
        if (skillListUp)
            return;

        if (currentMana < currentSpell.manaCost)
        {
            if (holding && Time.time - lastManaDisplayMessage >= 1f)
            {
                Debug.Log("Not enough mana");
                lastManaDisplayMessage = Time.time;
            }
            return;
        }

        if (currentSpell.onCooldown)
        {
            if (holding && Time.time - lastCooldownDisplayMessage >= 1f)
            {
                Debug.Log("On cooldown");
                lastCooldownDisplayMessage = Time.time;
            }
            return;
        }

        // If selected Spell is a channel spell
        if (currentSpell.channel)
        {
            Fire2(holding);
        }
        else
        {
            Fire1(holding);
        }

        lastCooldownDisplayMessage = Time.time;
        lastManaDisplayMessage = Time.time;
    }

    public void Cancel()
    {
        if (casting)
        {
            currentSpell.CancelCast();
            Fire(false);
            UIEventSystem.current.CancelSkill(currentSpell.uniqueOverlayToWeaponAdapterId, OverlayControls.skillFreezeAfterCasting);
        }
    }

    private void Fire1(bool charge)
    {
        if (canCast & charge && !currentSpell.onCooldown)
        {
            canCast = false;
            castingBasic = true;
            casting = true;
            canRelease = true;
            //start playing charging animation
            animationController.ChargeBasic(currentSpell.GetSource());
            currentSpell.CastSpell(simpleFirePoint, gameObject.transform, true, gameObject.name);
        }
        else if (!canCast && canRelease && castingBasic)
        {
            // Starts the cooldown of the released spell
            currentSpell.StartCooldown();
            UIEventSystem.current.FreezeAllSkills(currentSpell.uniqueOverlayToWeaponAdapterId, OverlayControls.skillFreezeAfterCasting);

            //start playing reseting animation
            animationController.ReleaseBasic();
            StartCoroutine(releaseFire1(castingAnimationSimple, castingAnimationSimpleReset));
        }
    }

    private void Fire2(bool holding)
    {
        if (canCast && !channeling && holding)
        {
            //start playing animation
            animationController.CastChannel(true, currentSpell.GetSource(), castingAnimationChannel, castingAnimationChannelReset);
            //start spell attack
            if (runningCoroutine != null) StopCoroutine(runningCoroutine);
            runningCoroutine = StartCoroutine(castFire2(castingAnimationChannel, true));
        }
        else if (channeling && !holding)
        {
            // Starts the cooldown of the released spell
            currentSpell.StartCooldown();
            UIEventSystem.current.FreezeAllSkills(currentSpell.uniqueOverlayToWeaponAdapterId, OverlayControls.skillFreezeAfterCasting);

            //start playing animation
            animationController.CastChannel(false, currentSpell.GetSource(), castingAnimationChannel, castingAnimationChannelReset);
            //stop spell attack
            if (runningCoroutine != null) StopCoroutine(runningCoroutine);
            runningCoroutine = StartCoroutine(castFire2(castingAnimationChannelReset, false));
        }
    }

    IEnumerator releaseFire1(float cast, float reset)
    {
        // Release spell after holding it
        canRelease = false;
        yield return new WaitForSeconds(cast);
        currentSpell.CastSpell(simpleFirePoint, false);
        animationController.HideSource();
        yield return new WaitForSeconds(reset);
        castingBasic = false;
        casting = false;
        canCast = true;
    }
    
    IEnumerator castFire2(float seconds, bool holding)
    {
        channeling = holding;
        if (holding)
        {
            // Start channel
            casting = true;
            yield return new WaitForSeconds(seconds);
            currentSpell.CastSpell(channelingFirePoint, true, gameObject.name);
        }
        else
        {
            // End channel
            currentSpell.CastSpell(channelingFirePoint, false);
            yield return new WaitForSeconds(seconds);
            casting = false;
        }
        canCast = !holding;
    }
}
