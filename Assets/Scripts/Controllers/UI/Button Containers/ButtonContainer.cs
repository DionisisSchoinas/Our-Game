using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class ButtonContainer : ElementHover, IPointerClickHandler
{
    [HideInInspector]
    public ButtonData buttonData;
    [HideInInspector]
    public OverlayControls overlayControls;
    [HideInInspector]
    public Button button;

    private void Awake()
    {
        button = gameObject.GetComponent<Button>();
        if (overlayControls == null)
            overlayControls = FindObjectOfType<OverlayControls>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Clicked();
    }

    public abstract void Clicked();
}
