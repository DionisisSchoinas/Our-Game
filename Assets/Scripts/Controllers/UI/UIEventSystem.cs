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

    public event Action<int, bool> onDragging;
    public void Dragging(int indexInAdapter, bool dragging)
    {
        if (onDragging != null)
        {
            onDragging(indexInAdapter, dragging);
        }
    }
}
