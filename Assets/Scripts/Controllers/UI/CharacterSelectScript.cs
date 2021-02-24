using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectScript : MonoBehaviour
{
    public CanvasGroup startMenu;

    private CanvasGroup canvasGroup;
    private Button wizard;
    private Button fighter;
    private Button backToMenu;

    private void Awake()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        OverlayControls.SetCanvasState(false, canvasGroup);

        Button[] buttons = gameObject.GetComponentsInChildren<Button>();
        wizard = buttons[0];
        fighter = buttons[1];
        backToMenu = buttons[2];

        wizard.onClick.AddListener(WizardPick);
        fighter.onClick.AddListener(FighterPick);
        backToMenu.onClick.AddListener(BackToMenu);
    }

    private void WizardPick()
    {

    }

    private void FighterPick()
    {

    }

    private void BackToMenu()
    {
        OverlayControls.SetCanvasState(true, startMenu);
        OverlayControls.SetCanvasState(false, canvasGroup);
    }
}
