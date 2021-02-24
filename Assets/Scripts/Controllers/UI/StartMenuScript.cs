using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuScript : MonoBehaviour
{
    public CanvasGroup characterSelect;

    private CanvasGroup canvasGroup;
    private Button startButton;
    private Button exitButton;

    private int mode;

    private void Awake()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        OverlayControls.SetCanvasState(true, canvasGroup);

        Button[] buttons = gameObject.GetComponentsInChildren<Button>();
        startButton = buttons[0];
        exitButton = buttons[1];

        startButton.onClick.AddListener(StartGameClick);
        exitButton.onClick.AddListener(ExitButtonClick);

        mode = -1;

        YesNoDialog.current.onResponded += Response;
    }

    private void OnDestroy()
    {
        YesNoDialog.current.onResponded -= Response;
    }

    private void StartGameClick()
    {
        OverlayControls.SetCanvasState(true, characterSelect);
        OverlayControls.SetCanvasState(false, canvasGroup);
    }

    private void ExitButtonClick()
    {
        mode = 0;
        OverlayControls.SetCanvasState(0.2f, canvasGroup);
        YesNoDialog.SetDialogText("Exit the Game");
    }

    private void Response(bool response)
    {
        if (response)
        {
            if (mode == 0)
            {
                ExitGame();
            }
        }
        OverlayControls.SetCanvasState(true, canvasGroup);
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
