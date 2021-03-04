using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillListButton : ButtonContainer, IPointerDownHandler, IPointerUpHandler
{
    public new void Awake()
    {
        base.Awake();

        UIEventSystem.current.onHighlightButtonInSkillList += Highlight;
        UIEventSystem.current.onUnhighlightButtonsInSkillList += UnHighlight;
    }

    public new void OnDestroy()
    {
        base.OnDestroy();
        UIEventSystem.current.onHighlightButtonInSkillList -= Highlight;
        UIEventSystem.current.onUnhighlightButtonsInSkillList -= UnHighlight;
    }

    private void Highlight(int indexInAdapter)
    {
        if (buttonData.skillIndexInAdapter == indexInAdapter)
        {
            buttonSelection.color = OverlayControls.selectedButtonColor;
        }
    }
    private void UnHighlight()
    {
        buttonSelection.color = OverlayControls.unselectedButtonColor;
    }

    private SkillListButton ReInstantiate()
    {
        SkillListButton btn = Instantiate(gameObject, parent).GetComponent<SkillListButton>();

        btn.buttonData = buttonData;
        btn.overlayControls = overlayControls;
        btn.parent = parent;
        btn.cooldownPercentage = cooldownPercentage;
        btn.skillListUp = skillListUp;

        btn.CheckCooldown();

        btn.transform.SetSiblingIndex(buttonData.skillIndexInColumn);
        return btn;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // If skill list not up
        if (!skillListUp)
            return;

        // Set new one to position
        ReInstantiate();
        // Get offset of mouse from position of transform
        clickPositionOffset = eventData.position - new Vector2(transform.position.x, transform.position.y);
        // Move to canvas to allow drag around
        transform.parent = canvas;


        rect.position = rect.position + Vector3.up * 3f + Vector3.left * 3f;

        // Notify event
        UIEventSystem.current.DraggingButton(this, true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // If skill list not up
        if (!skillListUp)
            return;

        // Notify event
        UIEventSystem.current.DraggingButton(this, false);
        // Destroy drag around button
        Destroy(gameObject);
    }
}
