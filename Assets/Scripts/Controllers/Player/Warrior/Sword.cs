using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public SkinnedMeshRenderer playerMesh;
    public GameObject swordObject;
    public Transform swordMotionRoot;
    public Transform tipPoint;
    public Transform basePoint;
    public SwordEffect defaultSwordEffect;
    public SwordEffect[] swordEffects;
    [HideInInspector]
    public bool isSwinging;

    private int selectedEffect;
    private SwordEffect currentEffect;
    private Renderer swordRenderer;

    private PlayerMovementScriptWarrior controls;
    private AnimationScriptControllerWarrior animator;
    public bool isCastingSkill;

    private void Start()
    {
        swordRenderer = swordObject.GetComponent<SkinnedMeshRenderer>();
        if (swordRenderer == null)
            swordRenderer = swordObject.GetComponent<MeshRenderer>();

        controls = GetComponent<PlayerMovementScriptWarrior>();
        animator = GetComponent<AnimationScriptControllerWarrior>();

        isSwinging = false;
        isCastingSkill = false;

        ChangeSwordEffect();
    }

    public SwordEffect GetDefaultSwordEffect()
    {
        return defaultSwordEffect;
    }

    public SwordEffect GetSelectedEffect()
    {
        if (selectedEffect == -1)
            return defaultSwordEffect;
        else
            return swordEffects[selectedEffect];
    }

    public bool SetSelectedSwordEffect(int value)
    {
        if (isSwinging)
            return false;

        selectedEffect = value;
        ChangeSwordEffect();
        return true;
    }

    public void Attack(AttackIndicator indicator, int comboPhase)
    {
        isSwinging = true;

        currentEffect.comboPhase = comboPhase;
        StartSwingTrail();
        currentEffect.StartSwingCooldown();
        currentEffect.Attack(controls, indicator, playerMesh);

        UIEventSystem.current.FreezeAllSkills(currentEffect.uniqueOverlayToWeaponAdapterId, currentEffect.swingCooldown * 0.5f);
    }

    private void ChangeSwordEffect()
    {
        if (currentEffect != null) Destroy(currentEffect.gameObject);

        if (selectedEffect == -1)
            currentEffect = defaultSwordEffect.InstaCast(controls, swordObject, playerMesh, swordRenderer, tipPoint, basePoint, swordMotionRoot);
        else
            currentEffect = swordEffects[selectedEffect].InstaCast(controls, swordObject, playerMesh, swordRenderer, tipPoint, basePoint, swordMotionRoot);

        currentEffect.transform.position = swordObject.transform.position;
        currentEffect.transform.rotation = swordObject.transform.rotation;

        StartCastAnimation();
    }

    private void StartCastAnimation()
    {
        isCastingSkill = true;
        animator.PlaySkillSelectAnimation();
        StartCoroutine(StopCastAnimation());
    }

    private IEnumerator StopCastAnimation()
    {
        yield return new WaitForSeconds(1f);
        animator.StopSkillSelectAnimation();
        isCastingSkill = false;
    }

    public void StartSwingTrail()
    {
        StartCoroutine(DelayBeforeTrail());
    }

    private IEnumerator DelayBeforeTrail()
    {
        yield return new WaitForSeconds(currentEffect.comboTrailTimings[currentEffect.comboPhase].delayToStartTrail);
        currentEffect.StartSwingTrail();
        yield return new WaitForSeconds(currentEffect.comboTrailTimings[currentEffect.comboPhase].delayToStopTrail);
        currentEffect.StopSwingTrail();
    }

    public List<SwordEffect> GetSwordEffects()
    {
        return swordEffects.ToList();
    }
}
