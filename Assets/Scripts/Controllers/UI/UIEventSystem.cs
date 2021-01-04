using System;
using UnityEngine;

public class UIEventSystem : MonoBehaviour
{
    public static UIEventSystem current;

    private void Awake()
    {
        current = this;
    }

    public event Action<bool> onHover;
    public void SetHover(bool hovering)
    {
        if (onHover != null)
        {
            onHover(hovering);
        }
    }

    public event Action<ButtonContainer, bool> onDraggingButton;
    public void DraggingButton(ButtonContainer buttonContainer, bool dragging)
    {
        if (onDraggingButton != null)
        {
            onDraggingButton(buttonContainer, dragging);
        }
    }

    public event Action<bool> onSkillListUp;
    public void SetSkillListUp(bool up)
    {
        if (onSkillListUp != null)
        {
            onSkillListUp(up);
        }
    }

    public event Action<int> onHighlightButtonInSkillList;
    public void SetHighlight(int indexInAdapter)
    {
        if (onHighlightButtonInSkillList != null)
        {
            onHighlightButtonInSkillList(indexInAdapter);
        }
    }

    public event Action onUnhighlightButtonsInSkillList;
    public void UnHighlightSKillList()
    {
        if (onHighlightButtonInSkillList != null)
        {
            onUnhighlightButtonsInSkillList();
        }
    }

    public event Action<float> onFreezeAllSkills;
    public void FreezeAllSkills(float delay)
    {
        if (onHighlightButtonInSkillList != null)
        {
            onFreezeAllSkills(delay);
        }
    }

    public event Action<int> onSkillPicked;
    public void SkillPicked(int skillIndexInAdapter)
    {
        if (onSkillPicked != null)
        {
            onSkillPicked(skillIndexInAdapter);
        }
    }

    public event Action<int> onSkillCast;
    public void SkillCast(int uniqueId)
    {
        if (onSkillCast != null)
        {
            onSkillCast(uniqueId);
        }
    }
    public event Action<float> onDodgeFinish;
    public void Dodged(float cooldown)
    {
        if (onDodgeFinish != null)
        {
            onDodgeFinish(cooldown);
        }
    }
}
