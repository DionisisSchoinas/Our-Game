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
    public static bool channeling;
    public static bool castingBasic;
    public static bool canRelease;

    private bool canCast;
    private int selectedSpell;
    private Coroutine runningCoroutine;

    private void Start()
    {
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

        //CancelHold();  ==== THIS SHOULD BE USED TO CANCEL THE SPELL NOT UST RELEASE

        // Release held spells
        if (castingBasic)
        {
            Fire1(false);
            StartCoroutine(ChangeSelectedIndex(castingAnimationSimple + castingAnimationSimpleReset / 2f, value));
            return;
        }
        if (channeling)
        {
            Fire2(false);
        }
        selectedSpell = value;
    }

    IEnumerator ChangeSelectedIndex(float delay, int value)
    {
        yield return new WaitForSeconds(delay);
        selectedSpell = value;
    }

    public void Fire1(bool charge)
    {
        if (canCast & charge)
        {
            canCast = false;
            castingBasic = true;
            canRelease = true;
            //start playing charging animation
            animationController.ChargeBasic(spells[selectedSpell].GetSource());
        }
        else if (!canCast && canRelease && castingBasic)
        {
            //start playing reseting animation
            animationController.ReleaseBasic();
            StartCoroutine(releaseFire1(castingAnimationSimple, castingAnimationSimpleReset));
        }
    }

    public void Fire2(bool holding)
    {
        if (canCast || channeling)
        {
            //start playing animation
            animationController.CastChannel(holding, spells[selectedSpell].GetSource(), castingAnimationChannel, castingAnimationChannelReset);
            //start spell attack
            if (runningCoroutine != null) StopCoroutine(runningCoroutine);
            runningCoroutine = StartCoroutine(castFire2( (holding ? castingAnimationChannel : castingAnimationChannelReset), holding));
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
        canRelease = false;
        yield return new WaitForSeconds(cast);
        spells[selectedSpell].FireSimple(simpleFirePoint);
        animationController.HideSource();
        yield return new WaitForSeconds(reset);
        castingBasic = false;
        canCast = true;
    }
    
    IEnumerator castFire2(float seconds, bool holding)
    {
        channeling = holding;
        if (holding)
        {
            yield return new WaitForSeconds(seconds);
            spells[selectedSpell].FireHold(holding, channelingFirePoint);
        }
        else
        {
            spells[selectedSpell].FireHold(holding, channelingFirePoint);
            yield return new WaitForSeconds(seconds);
        }
        canCast = !holding;
    }
}
