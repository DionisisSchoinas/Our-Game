using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonContainer : ElementHover
{
    [HideInInspector]
    public ButtonData buttonData;
    [HideInInspector]
    public OverlayControls overlayControls;
    [HideInInspector]
    public Button button;
    [HideInInspector]
    public Transform parent;

    protected RectTransform rect;
    protected Transform canvas;
    protected ColorBlock selectedColorBlock;

    public void Awake()
    {
        button = gameObject.GetComponent<Button>();
        if (overlayControls == null)
            overlayControls = FindObjectOfType<OverlayControls>();

        rect = GetComponent<RectTransform>();
        canvas = FindObjectOfType<OverlayControls>().transform;

        selectedColorBlock = ColorBlock.defaultColorBlock;
        selectedColorBlock.normalColor = Color.red;
        selectedColorBlock.highlightedColor = Color.magenta;
    }
}
