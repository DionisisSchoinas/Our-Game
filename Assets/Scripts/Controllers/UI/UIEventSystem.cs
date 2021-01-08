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
        if (onUnhighlightButtonsInSkillList != null)
        {
            onUnhighlightButtonsInSkillList();
        }
    }

    public event Action<int, float> onFreezeAllSkills;
    public void FreezeAllSkills(int uniqueAdapterIndex, float delay)
    {
        if (onFreezeAllSkills != null)
        {
            onFreezeAllSkills(uniqueAdapterIndex, delay);
        }
    }

    public event Action<int, float> onCancelSkill;
    public void CancelSkill(int uniqueAdapterIndex, float delay)
    {
        if (onCancelSkill != null)
        {
            onCancelSkill(uniqueAdapterIndex, delay);
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

    public event Action<int> onSkillPickedRegistered;
    public void SkillPickedRegister(int skillIndexInAdapter)
    {
        if (onSkillPickedRegistered != null)
        {
            onSkillPickedRegistered(skillIndexInAdapter);
        }
    }

    public event Action<int, float> onSkillCast;
    public void SkillCast(int uniqueId, float cooldown)
    {
        if (onSkillCast != null)
        {
            onSkillCast(uniqueId, cooldown);
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


    public event Action<Skill, float> onStartCooldown;
    public void StartCooldown(Skill skill, float delay)
    {
        if (onStartCooldown != null)
        {
            onStartCooldown(skill, delay);
        }
    }

    public event Action<string, float> onApplyResistance;
    public void ApplyResistance(string resistanceName, float duration)
    {
        if (onApplyResistance != null)
        {
            onApplyResistance(resistanceName, duration);
        }
    }

    public event Action onRemoveResistance;
    public void RemoveResistance()
    {
        if (onRemoveResistance != null)
        {
            onRemoveResistance();
        }
    }
}
