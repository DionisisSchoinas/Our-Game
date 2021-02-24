using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        SelectedCharacterScript[] scripts = FindObjectsOfType<SelectedCharacterScript>();
        foreach (SelectedCharacterScript s in scripts)
            Destroy(s.gameObject);

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
        GameObject gm = new GameObject();
        SelectedCharacterScript script = gm.AddComponent<SelectedCharacterScript>();
        script.SetCharacter(SelectedCharacterScript.Character.Wizard);
        ChangeToGameScene();
    }

    private void FighterPick()
    {
        GameObject gm = new GameObject();
        SelectedCharacterScript script = gm.AddComponent<SelectedCharacterScript>();
        script.SetCharacter(SelectedCharacterScript.Character.Fighter);
        ChangeToGameScene();
    }

    private void ChangeToGameScene()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    private void BackToMenu()
    {
        OverlayControls.SetCanvasState(true, startMenu);
        OverlayControls.SetCanvasState(false, canvasGroup);
    }
}
