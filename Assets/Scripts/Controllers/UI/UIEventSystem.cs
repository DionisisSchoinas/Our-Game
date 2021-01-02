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
}
