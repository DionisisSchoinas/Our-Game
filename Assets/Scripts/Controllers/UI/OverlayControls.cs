using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class OverlayControls : MonoBehaviour
{
    public GameObject buttonQuickbar;
    public CanvasGroup spellListDisplay;
    public GameObject columnContentHolder;
    public GameObject dodgeDisplay;
    public GameObject effectsDisplay;
    public float secondsAfterPickingSkill = 0.02f;
    public float secondsAfterCastingSkill = 0.02f;
    public Color buttonColorSelected;
    public CanvasGroup escapeMenu;

    // Quickbar data
    [HideInInspector]
    public Button[] quickbarButtons;
    [HideInInspector]
    public RectTransform[] quickbarButtonTransforms;
    [HideInInspector]
    public QuickbarButton[] quickbarButtonContainers;
    [HideInInspector]
    public OverlayToWeaponAdapter overlayToWeaponAdapter;

    private SkillListFill skillList;
    private int selectedQuickbarIndex;
    private bool skillListUp;
    private bool escapeMenuUp;

    public static float skillFreezeAfterPicking;
    public static float skillFreezeAfterCasting;
    public static Color selectedButtonColor;


    private void Start()
    {
        overlayToWeaponAdapter = FindObjectOfType<OverlayToWeaponAdapter>();

        skillList = gameObject.AddComponent<SkillListFill>();
        skillList.weaponAdapter = overlayToWeaponAdapter;
        skillList.overlayControls = this;
        skillList.columnContentHolder = columnContentHolder;

        skillList.FillList();

        SetCanvasState(false, spellListDisplay);
        SetCanvasState(false, escapeMenu);
        spellListDisplay.gameObject.AddComponent<ElementHover>();

        quickbarButtons = buttonQuickbar.GetComponentsInChildren<Button>();

        for (int i = 0; i < quickbarButtons.Length; i++)
        {
            if (quickbarButtons[i] == null)
            {
                Debug.LogError("Quickbar needs at least 5 buttons");
                break;
            }
        }

        quickbarButtonContainers = new QuickbarButton[quickbarButtons.Length];
        quickbarButtonTransforms = new RectTransform[quickbarButtons.Length];
        for (int i=0; i<quickbarButtons.Length; i++)
        {
            Text butttonText = quickbarButtons[i].GetComponentInChildren<Text>();
            // Put container script on the quickbar buttons
            quickbarButtonContainers[i] = quickbarButtons[i].gameObject.AddComponent<QuickbarButton>();
            Skill skill;
            if (i == 0) // Set to 1st quickbar button the default skill
            {
                skill = overlayToWeaponAdapter.GetSkillFromIndex(-1);
                quickbarButtonContainers[i].swappable = false;
                // Save values on the buttons script
                quickbarButtonContainers[i].buttonData = new ButtonData(quickbarButtons[i], skill, i, -1, butttonText);
            }
            else
            {
                skill = overlayToWeaponAdapter.GetSkillFromIndex(i-1);
                // Save values on the buttons script
                quickbarButtonContainers[i].buttonData = new ButtonData(quickbarButtons[i], skill, i, i-1, butttonText);
            }
            quickbarButtonContainers[i].overlayControls = this;
            quickbarButtonContainers[i].parent = buttonQuickbar.transform;

            // Transforms
            quickbarButtonTransforms[i] = quickbarButtons[i].GetComponent<RectTransform>();
        }

        skillFreezeAfterPicking = secondsAfterPickingSkill;
        skillFreezeAfterCasting = secondsAfterCastingSkill;
        selectedButtonColor = buttonColorSelected;

        // Hightlight the quickbar skills in the skill list
        HighlightQuickbarInList();

        skillListUp = false;
        escapeMenuUp = false;

        SetSelectedQuickBar(0);

        UIEventSystem.current.onDraggingButton += DraggingButton;
        UIEventSystem.current.onApplyResistance += ApplyResistance;
    }

    private void OnDestroy()
    {
        UIEventSystem.current.onDraggingButton -= DraggingButton;
        UIEventSystem.current.onApplyResistance -= ApplyResistance;
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
            if (skillListUp)
            {
                ChangeSkillListState();
            }
            else
            {
                EscapeMenu();
            }
        }

        // Spell List
        if (Input.GetKeyDown(KeyCode.K))
        {
            ChangeSkillListState();
        }
    }

    private void ApplyResistance(string resistanceName, float duration)
    {
        EffectDisplayContainer resistanceEffect = Instantiate(ResourceManager.UI.EffectDisplay, effectsDisplay.transform).AddComponent<EffectDisplayContainer>();
        resistanceEffect.SetResistanceText(resistanceName);
        resistanceEffect.StartCountdown(duration);
    }

    private int HoveringQuickbarButtons()
    {
        for (int i = 0; i < quickbarButtonTransforms.Length; i++)
        {
            if (quickbarButtonContainers[i].swappable)
            {
                Vector2 localMousePosition = quickbarButtonTransforms[i].InverseTransformPoint(Input.mousePosition);
                if (quickbarButtonTransforms[i].rect.Contains(localMousePosition))
                {
                    return i;
                }
            }
        }
        return -1;
    }

    private void HighlightQuickbarInList()
    {
        List<int> indexInAdapter = quickbarButtonContainers.Select(cont => cont.buttonData.skillIndexInAdapter).ToList();

        skillList.HightlightQuickbarSkills(indexInAdapter);
    }

    private void ChangeSkillListState()
    {
        skillListUp = !skillListUp;
        UIEventSystem.current.SetHover(skillListUp);
        UIEventSystem.current.SetSkillListUp(skillListUp);
        SetCanvasState(skillListUp, spellListDisplay);

        if (!skillListUp)
            SetSelectedQuickBar(selectedQuickbarIndex);
    }

    public static void SetCanvasState(bool show, CanvasGroup canvasGroup)
    {
        if (show)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public static void SetCanvasState(float alpha, CanvasGroup canvasGroup)
    {
        if (alpha > 0f)
        {
            canvasGroup.alpha = Mathf.Min(alpha, 1f);
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    private void DraggingButton(ButtonContainer buttonContainer, bool dragging)
    {
        int hoveringOverButton = HoveringQuickbarButtons();

        if (dragging || hoveringOverButton == -1)
            return;

        // Skill list button
        if (buttonContainer.buttonData.quickBarIndex == -1)
        {
            BindSkillToQuickbar(buttonContainer, hoveringOverButton);
        }
        // Quickbar button
        else
        {
            // If hovering a quickbar button and it's not the same button
            if (hoveringOverButton != buttonContainer.buttonData.quickBarIndex)
            {
                // Set the hovering button to the button we are dragging around
                BindSkillToQuickbar(quickbarButtonContainers[hoveringOverButton], buttonContainer.buttonData.quickBarIndex);
                // Set the button we are dragging around to the hovering button
                BindSkillToQuickbar(buttonContainer, hoveringOverButton);
            }
        }
    }

    // Binds the container data to the button with this index -> selectedQuickbar
    private void BindSkillToQuickbar(ButtonContainer container, int selectedQuickbar)
    {
        quickbarButtonContainers[selectedQuickbar].buttonData.NewData(container);
        HighlightQuickbarInList();
    }

    public void SetSelectedQuickBar(int selectedQuickbar)
    {
        if (!skillListUp)
        {
            selectedQuickbarIndex = selectedQuickbar;
            // Update Adapter
            UIEventSystem.current.SkillPicked(quickbarButtonContainers[selectedQuickbar].buttonData.skillIndexInAdapter);
        }
    }

    private void EscapeMenu()
    {
        SetCanvasState(escapeMenuUp, escapeMenu);
        PauseGame(escapeMenuUp);
        escapeMenuUp = !escapeMenuUp;
    }

    private void PauseGame(bool pause)
    {
        if (pause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
