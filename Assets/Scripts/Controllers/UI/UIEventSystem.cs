using System;
using UnityEngine;

public class UIEventSystem : MonoBehaviour
{
    public static UIEventSystem current;

    private void Awake()
    {
        current = this;
    }

    public event Action<string, bool> onHover;
    public void SetHover(string name, bool hovering)
    {
        if (onHover != null)
        {
            onHover(name, hovering);
        }
    }
}
