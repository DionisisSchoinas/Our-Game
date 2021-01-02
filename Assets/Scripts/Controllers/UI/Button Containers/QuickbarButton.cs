using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickbarButton : ButtonContainer, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [HideInInspector]
    public bool skillListUp;

    private Vector2 clickPositionOffset;
    private Vector2 lastPosition;

    public new void Awake()
    {
        base.Awake();
        skillListUp = false;
    }

    public void Start()
    {
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
    
    private QuickbarButton ReInstantiate()
    {
        // Instatiate new button and get components
        GameObject btnGm = Instantiate(gameObject, parent);
        Button btn = btnGm.GetComponent<Button>();
        QuickbarButton btnScript = btnGm.GetComponent<QuickbarButton>();

        // Set new values in script
        btnScript.buttonData = new ButtonData();
        btnScript.buttonData.CopyData(btn, buttonData);
        btnScript.skillListUp = skillListUp;
        btnScript.overlayControls = overlayControls;
        btnScript.parent = parent;
        btnScript.rect.position = lastPosition;

        // Set values back in OverlayControls
        overlayControls.quickbarButtons[buttonData.quickBarIndex] = btnScript.button;
        overlayControls.quickbarButtonContainers[buttonData.quickBarIndex] = btnScript;
        overlayControls.quickbarButtonTransforms[buttonData.quickBarIndex] = btnScript.rect;

        return btnScript;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (skillListUp)
        {
            Vector2 currentMousePosition = eventData.position;
            Vector3 oldPos = rect.position;
            rect.position = currentMousePosition - clickPositionOffset;
            if (!IsRectTransformInsideSreen())
            {
                rect.position = oldPos;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (skillListUp)
        {
            clickPositionOffset = eventData.position - new Vector2(transform.position.x, transform.position.y);
            transform.parent = canvas;
            lastPosition = rect.position;

            UIEventSystem.current.DraggingButton(this, true);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (skillListUp)
        {
            ReInstantiate();

            UIEventSystem.current.DraggingButton(this, false);

            Destroy(gameObject);
            
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
}
