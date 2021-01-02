using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class ButtonContainer : ElementHover
{
    [HideInInspector]
    public ButtonData buttonData;
    [HideInInspector]
    public OverlayControls overlayControls;
    [HideInInspector]
    public Button button;

    public void Awake()
    {
        button = gameObject.GetComponent<Button>();
        if (overlayControls == null)
            overlayControls = FindObjectOfType<OverlayControls>();
    }
}
