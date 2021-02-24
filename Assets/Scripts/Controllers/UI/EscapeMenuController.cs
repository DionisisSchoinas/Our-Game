using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscapeMenuController : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private Button exitToMenu;
    private Button exitGame;

    private int mode;

    private void Awake()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();

        Button[] buttons = gameObject.GetComponentsInChildren<Button>();
        exitToMenu = buttons[0];
        exitGame = buttons[1];

        exitToMenu.onClick.AddListener(ExitToMenuClick);
        exitGame.onClick.AddListener(ExitGameClick);

        mode = -1;

        YesNoDialog.current.onResponded += Response;
    }

    private void OnDestroy()
    {
        YesNoDialog.current.onResponded -= Response;
    }

    private void ExitToMenuClick()
    {
        mode = 0;
        OverlayControls.SetCanvasState(0.2f, canvasGroup);
        YesNoDialog.SetDialogText("Exit to the Main Menu");
    }

    private void ExitGameClick()
    {
        mode = 1;
        OverlayControls.SetCanvasState(0.2f, canvasGroup);
        YesNoDialog.SetDialogText("Exit the Game");
    }

    private void Response(bool response)
    {
        if (response)
        {
            switch (mode)
            {
                case 0:  // Back to main menu
                    ExitToMenu();
                    break;
                case 1:  // Exit game
                    ExitGame();
                    break;
            }
        }
        OverlayControls.SetCanvasState(true, canvasGroup);
    }

    private void ExitToMenu()
    {
        Time.timeScale = 1;
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene("StartMenuScene");
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
