using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillListButton : ButtonContainer, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private Vector2 clickPositionOffset;

    public new void Awake()
    {
        base.Awake();
        UIEventSystem.current.highlightButtonInSkillList += Highlight;
        UIEventSystem.current.unhighlightButtonsInSkillList += UnHighlight;
    }

    private void OnDestroy()
    {
        UIEventSystem.current.highlightButtonInSkillList -= Highlight;
        UIEventSystem.current.unhighlightButtonsInSkillList -= UnHighlight;
    }

    private void Highlight(int indexInAdapter)
    {
        if (buttonData.skillIndexInAdapter == indexInAdapter)
        {
            button.colors = selectedColorBlock;
        }
    }
    private void UnHighlight()
    {
        button.colors = ColorBlock.defaultColorBlock;
    }

    private SkillListButton ReInstantiate()
    {
        SkillListButton btn = Instantiate(gameObject, parent).GetComponent<SkillListButton>();
        btn.buttonData = buttonData;
        btn.overlayControls = overlayControls;
        btn.parent = parent;
        btn.transform.SetSiblingIndex(buttonData.skillIndexInColumn);
        return btn;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentMousePosition = eventData.position;
        Vector3 oldPos = rect.position;
        rect.position = currentMousePosition - clickPositionOffset;
        if (!IsRectTransformInsideSreen())
        {
            rect.position = oldPos;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Set new one to position
        ReInstantiate();
        // Get offset of mouse from position of transform
        clickPositionOffset = eventData.position - new Vector2(transform.position.x, transform.position.y);
        // Move to canvas to allow drag around
        transform.parent = canvas;
        // Notify event
        UIEventSystem.current.DraggingButton(this, true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Notify event
        UIEventSystem.current.DraggingButton(this, false);
        // Destroy drag around button
        Destroy(gameObject);
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
}
