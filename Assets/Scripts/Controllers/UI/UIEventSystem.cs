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

    public event Action<bool> skillListUp;
    public void SetSkillListUp(bool up)
    {
        if (skillListUp != null)
        {
            skillListUp(up);
        }
    }

    public event Action<int> highlightButtonInSkillList;
    public void SetHighlight(int indexInAdapter)
    {
        if (highlightButtonInSkillList != null)
        {
            highlightButtonInSkillList(indexInAdapter);
        }
    }

    public event Action unhighlightButtonsInSkillList;
    public void UnHighlightSKillList()
    {
        if (highlightButtonInSkillList != null)
        {
            unhighlightButtonsInSkillList();
        }
    }

    public event Action onSkillPicked;
    public void SkillPicked()
    {
        if (onSkillPicked != null)
        {
            onSkillPicked();
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
}
