using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuickbarButton : ButtonContainer, IPointerClickHandler//, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private Vector2 clickPositionOffset;
    private bool skillListUp;

    public new void Awake()
    {
        base.Awake();
        skillListUp = false;
        UIEventSystem.current.skillListUp += BlockQuickbarSkillSelection;
    }

    private void OnDestroy()
    {
        UIEventSystem.current.skillListUp -= BlockQuickbarSkillSelection;
    }

    private void BlockQuickbarSkillSelection(bool block)
    {
        skillListUp = block;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!skillListUp)
            overlayControls.SetSelectedQuickBar(buttonData.quickBarIndex);
    }
    /*
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
        ReInstantiate();

        clickPositionOffset = eventData.position - new Vector2(transform.position.x, transform.position.y);
        transform.parent = canvas;

        UIEventSystem.current.DraggingButton(this, true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        UIEventSystem.current.DraggingButton(this, false);

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
    */
}
