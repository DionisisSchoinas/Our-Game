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
    public bool coolingDown;
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

        coolingDown = false;

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
        StartCoroutine(StartCooldown());
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
        if (!coolingDown && isActiveAndEnabled)
        {
            if (!buttonData.skill.onCooldown)
                buttonData.skill.StartCooldownWithoutEvent(overlayControls.secondsAfterPickingSkill);

            StartCoroutine(StartCooldown());
        }
    }

    private void SkillCast(int uniqueAdapterId)
    {
        if (!coolingDown && buttonData.skill.uniqueOverlayToWeaponAdapterId == uniqueAdapterId && isActiveAndEnabled)
        {
            StartCoroutine(StartCooldown());
        }
    }

    private void Freeze(int uniqueAdapterIndex, float delay)
    {
        if (!coolingDown && isActiveAndEnabled && buttonData.skill.uniqueOverlayToWeaponAdapterId != uniqueAdapterIndex)
        {
            if (!buttonData.skill.onCooldown)
                buttonData.skill.StartCooldownWithoutEvent(delay);

            StartCoroutine(StartCooldown());
        }
    }

    private IEnumerator StartCooldown()
    {
        coolingDown = true;
        float delayForEachStep = buttonData.skill.activeCooldown / 100f;
        while (buttonData.skill.onCooldown)
        {
            buttonImageCooldown.fillAmount = buttonData.skill.cooldownPercentage;
            yield return new WaitForSeconds(delayForEachStep / 2f);
        }
        buttonImageCooldown.fillAmount = 0f;
        coolingDown = false;

        yield return null;
    }
}
