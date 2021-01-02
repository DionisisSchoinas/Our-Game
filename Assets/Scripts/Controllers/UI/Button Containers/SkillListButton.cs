using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillListButton : ButtonContainer, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Transform parent;

    private RectTransform rect;
    private Transform canvas;

    private Vector2 clickPositionOffset;

    public new void Awake()
    {
        base.Awake();
        rect = GetComponent<RectTransform>();
        canvas = FindObjectOfType<OverlayControls>().transform;
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
}
