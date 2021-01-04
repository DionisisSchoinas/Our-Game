﻿using UnityEngine;
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

        UIEventSystem.current.onSkillPicked += SkillUsed;
        UIEventSystem.current.onSkillCast += SkillCast;
        UIEventSystem.current.onFreezeAllSkills += Freeze;
    }


    public void OnDestroy()
    {
        UIEventSystem.current.onSkillPicked -= SkillUsed;
        UIEventSystem.current.onSkillCast -= SkillCast;
        UIEventSystem.current.onFreezeAllSkills -= Freeze;
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
    private void SkillUsed(int skillIndexInAdapter)
    {
        if (!coolingDown && isActiveAndEnabled)
        {
            StartCoroutine(StartCooldown(overlayControls.secondsAfterPickingSkill));
        }
    }

    private void SkillCast(int uniqueAdapterId)
    {
        if (buttonData.skill.uniqueOverlayToWeaponAdapterId == uniqueAdapterId && isActiveAndEnabled)
        {
            StartCoroutine(StartCooldown(buttonData.skill.cooldown));
        }
    }

    private void Freeze(float delay)
    {
        if (!coolingDown && isActiveAndEnabled)
        {
            StartCoroutine(StartCooldown(delay));
        }
    }

    private IEnumerator StartCooldown(float cooldown)
    {
        coolingDown = true;
        float i = 0f;
        float delayForEachStep = cooldown / 50f;
        while (i < 1)
        {
            i += 0.02f;
            buttonImageCooldown.fillAmount += 0.02f;
            yield return new WaitForSeconds(delayForEachStep);
        }
        buttonImageCooldown.fillAmount = 0;
        coolingDown = false;

        yield return null;
    }
}
