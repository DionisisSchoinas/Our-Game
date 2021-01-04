using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickbarButton : ButtonContainer, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [HideInInspector]
    public bool skillListUp;

    private Vector2 lastPosition;

    public new void Awake()
    {
        base.Awake();
        skillListUp = false;
    }

    public void Start()
    {
        UIEventSystem.current.onSkillListUp += BlockQuickbarSkillSelection;
    }

    public new void OnDestroy()
    {
        base.OnDestroy();
        UIEventSystem.current.onSkillListUp -= BlockQuickbarSkillSelection;
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

        btnScript.transform.SetSiblingIndex(btnScript.buttonData.quickBarIndex);

        return btnScript;
    }

    public new void OnDrag(PointerEventData eventData)
    {
        if (skillListUp)
        {
            Drag(eventData);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (skillListUp)
        {
            ReInstantiate();

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
            UIEventSystem.current.DraggingButton(this, false);

            Destroy(gameObject);
            
        }
    }
}
