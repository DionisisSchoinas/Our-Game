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
    public float delayUiAfterCasting;

    private bool canCast;
    private int selectedSpell;
    private Coroutine runningCoroutine;

    private void Start()
    {
        casting = false;

        castingBasic = false;
        channeling = false;
        canCast = true;
        canRelease = false;

        foreach (Spell s in spells)
        {
            s.WakeUp();
        }
    }

    public List<Spell> GetSpells()
    {
        return spells.ToList();
    }

    public Spell GetSelectedSpell()
    {
        return spells[selectedSpell];
    }

    public void SetSelectedSpell(int value)
    {
        // Release held spells
        if (castingBasic)
        {
            Cancel();
            StartCoroutine(ChangeSelectedIndex(castingAnimationSimple + castingAnimationSimpleReset / 2f, value));
            return;
        }
        if (channeling)
        {
            Cancel();
        }
        SetSpellIndex(value);
    }

    private void SetSpellIndex(int value)
    {
        selectedSpell = value;
    }

    IEnumerator ChangeSelectedIndex(float delay, int value)
    {
        yield return new WaitForSeconds(delay);
        SetSpellIndex(value);
    }

    public void Fire(bool holding)
    {
        // If selected Spell is a channel spell
        if (spells[selectedSpell].channel)
        {
            Fire2(holding);
        }
        else
        {
            Fire1(holding);
        }
    }

    public void Cancel()
    {
        if (casting)
        {
            spells[selectedSpell].CancelCast();
            Fire(false);
        }
    }

    private void Fire1(bool charge)
    {
        if (canCast & charge && !spells[selectedSpell].onCooldown)
        {
            canCast = false;
            castingBasic = true;
            casting = true;
            canRelease = true;
            //start playing charging animation
            animationController.ChargeBasic(spells[selectedSpell].GetSource());
        }
        else if (!canCast && canRelease && castingBasic)
        {
            // Starts the cooldown of the released spell
            spells[selectedSpell].StartCooldown();
            UIEventSystem.current.FreezeAllSkills(delayUiAfterCasting);

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
            animationController.CastChannel(true, spells[selectedSpell].GetSource(), castingAnimationChannel, castingAnimationChannelReset);
            //start spell attack
            if (runningCoroutine != null) StopCoroutine(runningCoroutine);
            runningCoroutine = StartCoroutine(castFire2( castingAnimationChannel, true));
        }
        else if (channeling && !holding)
        {
            // Starts the cooldown of the released spell
            spells[selectedSpell].StartCooldown();
            UIEventSystem.current.FreezeAllSkills(delayUiAfterCasting);

            //start playing animation
            animationController.CastChannel(false, spells[selectedSpell].GetSource(), castingAnimationChannel, castingAnimationChannelReset);
            //stop spell attack
            if (runningCoroutine != null) StopCoroutine(runningCoroutine);
            runningCoroutine = StartCoroutine(castFire2(castingAnimationChannelReset, false));
        }
    }
    /*
    private void CancelHold()
    {
        if (castingBasic)
            StartCoroutine(cancelFire1(castingAnimationSimpleReset));
        if (channeling)
            StartCoroutine(cancelFire2(castingAnimationChannelReset));
    }

    IEnumerator cancelFire1(float reset)
    {
        animationController.HideSource();
        yield return new WaitForSeconds(reset);
        castingBasic = false;
        canCast = true;
    }
    IEnumerator cancelFire2(float reset)
    {
        spells[selectedSpell].FireHold(false, channelingFirePoint);
        yield return new WaitForSeconds(reset);
        canCast = true;
    }
    */
    IEnumerator releaseFire1(float cast, float reset)
    {
        // Release spell after holding it
        canRelease = false;
        yield return new WaitForSeconds(cast);
        spells[selectedSpell].CastSpell(simpleFirePoint, false);
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
            spells[selectedSpell].CastSpell(channelingFirePoint, true);
        }
        else
        {
            // End channel
            spells[selectedSpell].CastSpell(channelingFirePoint, false);
            yield return new WaitForSeconds(seconds);
            casting = false;
        }
        canCast = !holding;
    }
}
