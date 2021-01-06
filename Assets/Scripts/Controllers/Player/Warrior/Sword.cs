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
    public SwordEffect[] swordEffects;

    public float delayBeforeSwing;
    public float delayBeforeStoppingSwing;

    private int selectedEffect;
    private SwordEffect currentEffect;
    private Renderer swordRenderer;

    private void Start()
    {
        swordRenderer = swordObject.GetComponent<SkinnedMeshRenderer>();
        if (swordRenderer == null)
            swordRenderer = swordObject.GetComponent<MeshRenderer>();

        selectedEffect = 0;
        ChangeSwordEffect();
    }

    public SwordEffect GetSelectedEffect()
    {
        return swordEffects[selectedEffect];
    }

    public void SetSelectedSwordEffect(int value)
    {
        selectedEffect = value;
        ChangeSwordEffect();
    }

    public void Attack(PlayerMovementScriptWarrior controls, AttackIndicator indicator)
    {
        currentEffect.StartSwingCooldown();
        currentEffect.Attack(controls, indicator, playerMesh);

        UIEventSystem.current.FreezeAllSkills(swordEffects[selectedEffect].uniqueOverlayToWeaponAdapterId, OverlayControls.skillFreezeAfterCasting);
    }

    private void ChangeSwordEffect()
    {
        if (currentEffect != null) Destroy(currentEffect.gameObject);

        currentEffect = swordEffects[selectedEffect].InstantiateEffect(tipPoint, basePoint, swordMotionRoot).GetComponent<SwordEffect>();
        currentEffect.transform.position = swordObject.transform.position;
        currentEffect.transform.rotation = swordObject.transform.rotation;

        swordRenderer.material = currentEffect.attributes.swordMaterial;
    }

    public void StartSwing()
    {
        StartCoroutine(DelayBeforeTrail());
    }

    private IEnumerator DelayBeforeTrail()
    {
        yield return new WaitForSeconds(delayBeforeSwing);
        currentEffect.StartSwing();
        yield return new WaitForSeconds(delayBeforeStoppingSwing);
        StopSwing();
    }

    public void StopSwing()
    {
        currentEffect.StopSwing();
    }

    public List<SwordEffect> GetSwordEffects()
    {
        return swordEffects.ToList();
    }
}
