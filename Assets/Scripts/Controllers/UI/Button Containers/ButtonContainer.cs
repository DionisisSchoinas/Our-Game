using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonContainer : ElementHover, IDragHandler
{
    [HideInInspector]
    public ButtonData buttonData;
    [HideInInspector]
    public OverlayControls overlayControls;
    [HideInInspector]
    public Button button;
    [HideInInspector]
    public Transform parent;
    [HideInInspector]
    public bool buttonAlreadyDisplayingCooldown;
    [HideInInspector]
    public float cooldownPercentage;

    protected RectTransform rect;
    protected Transform canvas;
    protected Image buttonBackground;
    protected Image buttonImageCooldown;
    protected Vector2 clickPositionOffset;

    public void Awake()
    {
        button = gameObject.GetComponent<Button>();

        if (overlayControls == null)
            overlayControls = FindObjectOfType<OverlayControls>();

        rect = GetComponent<RectTransform>();
        canvas = FindObjectOfType<OverlayControls>().transform;

        buttonBackground = gameObject.GetComponent<Image>();

        buttonImageCooldown = gameObject.GetComponentsInChildren<Image>()[1];
        buttonImageCooldown.fillAmount = 0;

        buttonAlreadyDisplayingCooldown = false;

        UIEventSystem.current.onSkillPicked += SkillPicked;
        UIEventSystem.current.onSkillCast += SkillCast;
        UIEventSystem.current.onFreezeAllSkills += Freeze;
    }


    public void OnDestroy()
    {
        UIEventSystem.current.onSkillPicked -= SkillPicked;
        UIEventSystem.current.onSkillCast -= SkillCast;
        UIEventSystem.current.onFreezeAllSkills -= Freeze;
    }

    //------------ Reset functions ------------
    public void CheckCooldown()
    {
        StartCoroutine(StartCooldown(buttonData.skill.cooldown));
    }


    //------------ Drag functions ------------
    public void OnDrag(PointerEventData eventData)
    {
        Drag(eventData);
    }

    protected void Drag(PointerEventData eventData)
    {
        Vector2 currentMousePosition = eventData.position;
        Vector3 oldPos = rect.position;
        rect.position = currentMousePosition - clickPositionOffset;
        if (!IsRectTransformInsideSreen())
        {
            rect.position = oldPos;
        }
    }

    private bool IsRectTransformInsideSreen()
    {
        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);
        Rect screen = new Rect(0, 0, Screen.width, Screen.height);
        foreach (Vector3 corner in corners)
        {
            if (!screen.Contains(corner))
            {
                return false;
            }
        }
        return true;
    }


    //------------ Event functions ------------
    private void SkillPicked(int skillIndexInAdapter)
    {
        if (!buttonAlreadyDisplayingCooldown && isActiveAndEnabled)
        {
            if (!buttonData.skill.onCooldown)
                buttonData.skill.StartCooldownWithoutEvent(overlayControls.secondsAfterPickingSkill);

            StartCoroutine(StartCooldown(overlayControls.secondsAfterPickingSkill));
        }
    }

    private void SkillCast(int uniqueAdapterId, float cooldown)
    {
        if (!buttonAlreadyDisplayingCooldown && buttonData.skill.uniqueOverlayToWeaponAdapterId == uniqueAdapterId && isActiveAndEnabled)
        {
            if (!buttonData.skill.onCooldown)
                buttonData.skill.StartCooldownWithoutEvent(cooldown);

            StartCoroutine(StartCooldown(cooldown));
        }
    }

    private void Freeze(int uniqueAdapterId, float delay)
    {
        if (!buttonAlreadyDisplayingCooldown && isActiveAndEnabled && buttonData.skill.uniqueOverlayToWeaponAdapterId != uniqueAdapterId)
        {
            if (!buttonData.skill.onCooldown)
                buttonData.skill.StartCooldownWithoutEvent(delay);

            StartCoroutine(StartCooldown(delay));
        }
    }

    private IEnumerator StartCooldown(float cooldown)
    {
        buttonAlreadyDisplayingCooldown = true;
        float delayForEachStep = cooldown / 100f;

        if (buttonData.skill.uniqueOverlayToWeaponAdapterId == 0)
            Debug.Log(buttonAlreadyDisplayingCooldown + " " + cooldown + " " + buttonData.skill.onCooldown);

        while (buttonData.skill.cooldownPercentage < 1)
        {
            buttonImageCooldown.fillAmount = buttonData.skill.cooldownPercentage;
            yield return new WaitForSeconds(delayForEachStep / 2f);
        }
        buttonImageCooldown.fillAmount = 0f;
        buttonAlreadyDisplayingCooldown = false;

        yield return null;
    }
}
