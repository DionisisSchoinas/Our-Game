using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YesNoDialog : MonoBehaviour
{
    public static YesNoDialog current;

    private static Text text;
    private static CanvasGroup canvasGroup;
    private Button no;
    private Button yes;


    private void Awake()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        OverlayControls.SetCanvasState(false, canvasGroup);

        text = gameObject.GetComponentInChildren<Text>();

        Button[] buttons = gameObject.GetComponentsInChildren<Button>();
        no = buttons[0];
        yes = buttons[1];

        no.onClick.AddListener(NoClicked);
        yes.onClick.AddListener(YesClicked);

        current = this;
    }

    public event Action<bool> onResponded;
    public void SetResponse(bool response)
    {
        if (onResponded != null)
        {
            onResponded(response);
        }
    }

    public static void SetDialogText(string message)
    {
        text.text = "Are you sure you want\nto " + message + " ?";
        OverlayControls.SetCanvasState(true, canvasGroup);
    }

    private void NoClicked()
    {
        current.SetResponse(false);
        OverlayControls.SetCanvasState(false, canvasGroup);
    }

    private void YesClicked()
    {
        current.SetResponse(true);
        OverlayControls.SetCanvasState(false, canvasGroup);
    }
}
