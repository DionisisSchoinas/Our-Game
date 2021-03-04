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
    public bool buttonAlreadyDisplayingCooldown;
    [HideInInspector]
    public float cooldownPercentage;

    protected RectTransform rect;
    protected Transform canvas;
    protected Image buttonSelection;
    protected Image buttonBackground;
    protected Image buttonImageCooldown;
    protected Image buttonOutOfMana;
    protected Vector2 clickPositionOffset;
    protected bool skillListUp;

    public void Awake()
    {
        button = gameObject.GetComponent<Button>();

        if (overlayControls == null)
            overlayControls = FindObjectOfType<OverlayControls>();

        rect = GetComponent<RectTransform>();
        canvas = FindObjectOfType<OverlayControls>().transform;

        Image[] images = gameObject.GetComponentsInChildren<Image>();
        // Highlight border
        buttonSelection = images[0];
        // Button background
        buttonBackground = images[1];
        // Spinning cooldown background
        buttonImageCooldown = images[2];
        buttonImageCooldown.fillAmount = 0;
        // Gray ouf of mana background
        buttonOutOfMana = images[3];
        buttonOutOfMana.fillAmount = 0;

        buttonAlreadyDisplayingCooldown = false;
        skillListUp = false;

        UIEventSystem.current.onSkillPickedRegistered += SkillPicked;
        UIEventSystem.current.onSkillCast += SkillCast;
        UIEventSystem.current.onFreezeAllSkills += Freeze;
        UIEventSystem.current.onSkillListUp += BlockQuickbarSkillSelection;
        UIEventSystem.current.onCancelSkill += SkillCast;
        ManaEventSystem.current.onManaUpdated += ManaUpdate;
    }


    public void OnDestroy()
    {
        UIEventSystem.current.onSkillPickedRegistered -= SkillPicked;
        UIEventSystem.current.onSkillCast -= SkillCast;
        UIEventSystem.current.onFreezeAllSkills -= Freeze;
        UIEventSystem.current.onSkillListUp -= BlockQuickbarSkillSelection;
        UIEventSystem.current.onCancelSkill -= SkillCast;
        ManaEventSystem.current.onManaUpdated -= ManaUpdate;
    }

    private void BlockQuickbarSkillSelection(bool block)
    {
        skillListUp = block;
    }

    public void SetSelectionColor(Color color)
    {
        buttonSelection.color = color;
    }

    //------------ Reset functions ------------
    public void CheckCooldown()
    {
        if (buttonData.skill.cooldownPercentage != 0)
        {
            StartCoroutine(StartCooldown(buttonData.skill.cooldown));
        }
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
    private void SkillPicked(int skillIndexInAdapter, bool startCooldown)
    {
        if (buttonData.skillIndexInAdapter == skillIndexInAdapter && !startCooldown)
            return;

        if (!buttonAlreadyDisplayingCooldown && isActiveAndEnabled)
        {
            if (!buttonData.skill.onCooldown)
                buttonData.skill.StartCooldownWithoutEvent(OverlayControls.skillFreezeAfterPicking);

            StartCoroutine(StartCooldown(OverlayControls.skillFreezeAfterPicking));
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

    private void ManaUpdate(float mana)
    {
        if (buttonData.skill.manaCost > mana)
        {
            buttonOutOfMana.fillAmount = 1f;
        }
        else if (buttonOutOfMana.fillAmount == 1f)
        {
            buttonOutOfMana.fillAmount = 0f;
        }
    }

    private IEnumerator StartCooldown(float cooldown)
    {
        buttonAlreadyDisplayingCooldown = true;
        float delayForEachStep = cooldown / 100f;

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
