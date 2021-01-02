using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayControls : MonoBehaviour
{
    public GameObject quickbar;
    public GameObject spellListDisplay;
    public GameObject columnContentHolder;
    // Quickbar data
    public Button[] quickbarButtons;
    private RectTransform[] quickbarButtonTransforms;
    private ButtonContainer[] quickbarButtonContainers;

    private OverlayToWeaponAdapter overlayToWeaponAdapter;
    private SkillListFill skillList;
    private ButtonContainer lastSelected;
    private ColorBlock selectedColorBlock;
    private bool paused;


    private void Start()
    {
        overlayToWeaponAdapter = FindObjectOfType<OverlayToWeaponAdapter>();

        selectedColorBlock = ColorBlock.defaultColorBlock;
        selectedColorBlock.normalColor = Color.red;
        selectedColorBlock.highlightedColor = Color.magenta;

        skillList = gameObject.AddComponent<SkillListFill>();
        skillList.weaponAdapter = overlayToWeaponAdapter;
        skillList.overlayControls = this;
        skillList.columnContentHolder = columnContentHolder;

        skillList.FillList();

        spellListDisplay.SetActive(false);
        spellListDisplay.gameObject.AddComponent<ElementHover>();

        if (quickbarButtons.Length < 5)
            Debug.LogError("Quickbar needs at least 5 buttons");

        quickbarButtonContainers = new ButtonContainer[quickbarButtons.Length];
        quickbarButtonTransforms = new RectTransform[quickbarButtons.Length];
        for (int i=0; i<quickbarButtons.Length; i++)
        {
            Text butttonText = quickbarButtons[i].GetComponentInChildren<Text>();
            Skill skill = overlayToWeaponAdapter.GetSkillFromIndex(i);
            // Put container script on the quickbar buttons
            quickbarButtonContainers[i] = quickbarButtons[i].gameObject.AddComponent<QuickbarButton>();
            // Save values on the buttons script
            quickbarButtonContainers[i].buttonData = new ButtonData(skill, i, i, butttonText);
            quickbarButtonContainers[i].overlayControls = this;
            quickbarButtonContainers[i].parent = quickbar.transform;

            // Transforms
            quickbarButtonTransforms[i] = quickbarButtons[i].GetComponent<RectTransform>();
        }

        ResetLastButton();

        UIEventSystem.current.onDraggingButton += DraggingButton;
    }

    private void OnDestroy()
    {
        UIEventSystem.current.onDraggingButton -= DraggingButton;
    }

    private void Update()
    {
        // Quick bar inptus
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            SetSelectedQuickBar(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            SetSelectedQuickBar(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            SetSelectedQuickBar(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            SetSelectedQuickBar(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
        {
            SetSelectedQuickBar(4);
        }

        // Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                ChangeSkillListState();
            }
            else
            {
                // Escape menu
            }
        }

        // Spell List
        if (Input.GetKeyDown(KeyCode.K))
        {
            ChangeSkillListState();
        }
    }

    private int HoveringQuickbarButtons()
    {
        for (int i = 0; i < quickbarButtonTransforms.Length; i++)
        {
            Vector2 localMousePosition = quickbarButtonTransforms[i].InverseTransformPoint(Input.mousePosition);
            if (quickbarButtonTransforms[i].rect.Contains(localMousePosition))
            {
                return i;
            }
        }
        return -1;
    }

    private void ChangeSkillListState()
    {
        paused = !paused;
        UIEventSystem.current.SetHover(paused);
        UIEventSystem.current.SetSkillListUp(paused);
        PauseGame(paused);
        spellListDisplay.SetActive(paused);

        ResetLastButton();
    }

    private void DraggingButton(ButtonContainer buttonContainer, bool dragging)
    {
        // Skill list button
        if (buttonContainer.buttonData.quickBarIndex == -1)
        {
            if (dragging)
            {
                //this.bindingSkillIndex = buttonContainer.buttonData.skillIndexInAdapter;
            }
            else
            {
                int hoveringOverButton = HoveringQuickbarButtons();
                if (hoveringOverButton != -1)
                {
                    BindSkillToQuickbar(buttonContainer, hoveringOverButton);
                }
                //this.bindingSkillIndex = -1;
            }
        }
        // Quickbar button
        else
        {

        }
    }

    private void BindSkillToQuickbar(ButtonContainer container, int selectedQuickbar)
    {
        quickbarButtonContainers[selectedQuickbar].buttonData.NewValues(container.buttonData.skill, container.buttonData.skillIndexInAdapter);
        ResetLastButton();
    }

    public void SetSelectedQuickBar(int selectedQuickbar)
    {
        // Update Adapter
        overlayToWeaponAdapter.SelectedOnQuickbar(quickbarButtonContainers[selectedQuickbar].buttonData.skillIndexInAdapter);


        //lastSelected = buttons[selectedQuickBar];
    }

    private void ResetLastButton()
    {
        if (lastSelected != null)
            lastSelected.button.colors = ColorBlock.defaultColorBlock;
        lastSelected = null;
    }

    private void PauseGame(bool pause)
    {
        if (pause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
