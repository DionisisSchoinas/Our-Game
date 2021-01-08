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

    

    private void Start()
    {
        swordRenderer = swordObject.GetComponent<SkinnedMeshRenderer>();
        if (swordRenderer == null)
            swordRenderer = swordObject.GetComponent<MeshRenderer>();

        isSwinging = false;

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

    public void Attack(PlayerMovementScriptWarrior controls, AttackIndicator indicator, int comboPhase)
    {
        isSwinging = true;

        currentEffect.comboPhase = comboPhase;
        StartSwingTrail();
        currentEffect.StartSwingCooldown();
        currentEffect.Attack(controls, indicator, playerMesh);

        if (selectedEffect == -1)
            UIEventSystem.current.FreezeAllSkills(defaultSwordEffect.uniqueOverlayToWeaponAdapterId, currentEffect.swingCooldown * 0.5f);
        else
            UIEventSystem.current.FreezeAllSkills(swordEffects[selectedEffect].uniqueOverlayToWeaponAdapterId, currentEffect.swingCooldown * 0.5f);
    }

    private void ChangeSwordEffect()
    {
        if (currentEffect != null) Destroy(currentEffect.gameObject);

        if (selectedEffect == -1)
            currentEffect = defaultSwordEffect.InstantiateEffect(tipPoint, basePoint, swordMotionRoot).GetComponent<SwordEffect>();
        else
            currentEffect = swordEffects[selectedEffect].InstantiateEffect(tipPoint, basePoint, swordMotionRoot).GetComponent<SwordEffect>();

        currentEffect.transform.position = swordObject.transform.position;
        currentEffect.transform.rotation = swordObject.transform.rotation;

        swordRenderer.material = currentEffect.attributes.swordMaterial;
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
