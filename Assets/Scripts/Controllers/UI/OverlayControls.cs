using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayControls : MonoBehaviour
{
    public struct ButtonData
    {
        public int quickBarIndex;
        public int skillIndexInAdapter;
        public Text buttonText;

        public ButtonData(int quickBarIndex, int skillIndexInAdapter, Text buttonText)
        {
            this.quickBarIndex = quickBarIndex;
            this.skillIndexInAdapter = skillIndexInAdapter;
            this.buttonText = buttonText;
        }
    }

    public OverlayToWeaponAdapter overlayToWeaponAdapter;
    public GameObject spellListDisplay;
    public GameObject columnContentHolder;
    // Quickbar data
    public Button[] quickbar;
    private ButtonData[] buttonData;

    private SkillListFill skillList;
    private Button[] buttons;
    private Button lastSelected;
    private int bindingSkillIndex;
    private ColorBlock selectedColorBlock;
    private bool paused;
    private bool binding;

    private void Start()
    {
        selectedColorBlock = ColorBlock.defaultColorBlock;
        selectedColorBlock.normalColor = Color.red;
        selectedColorBlock.highlightedColor = Color.magenta;

        buttons = GetComponentsInChildren<Button>();
        foreach (Button obj in buttons)
        {
            obj.gameObject.AddComponent<ElementHover>();
        }

        skillList = gameObject.AddComponent<SkillListFill>();
        skillList.weaponAdapter = overlayToWeaponAdapter;
        skillList.overlayControls = this;
        skillList.columnContentHolder = columnContentHolder;

        skillList.FillList();

        spellListDisplay.SetActive(false);
        spellListDisplay.gameObject.AddComponent<ElementHover>();

        binding = false;

        if (quickbar.Length < 5)
            Debug.LogError("Quickbar needs at least 5 buttons");

        buttonData = new ButtonData[quickbar.Length + 1];
        for (int i=0; i<quickbar.Length; i++)
        {
            Text butttonText = quickbar[i].GetComponentInChildren<Text>();
            butttonText.text = overlayToWeaponAdapter.GetSkillFromIndex(i).Name;
            buttonData[i] = new ButtonData(i, i, butttonText);
        }
    }

    private void Update()
    {
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

        if (Input.GetKeyDown(KeyCode.K))
        {
            paused = !paused;
            UIEventSystem.current.SetHover(gameObject.name, paused);
            PauseGame(paused);
            spellListDisplay.SetActive(paused);

            ResetLastButton();
        }
    }

    public void PickingKeyBind(int indexInAdapter, Button button)
    {
        button.colors = selectedColorBlock;
        bindingSkillIndex = indexInAdapter;
        lastSelected = button;
        binding = true;
    }

    public void SetSelectedQuickBar(int selectedQuickBar)
    {
        if (binding)
        {
            buttonData[selectedQuickBar].skillIndexInAdapter = bindingSkillIndex;
            buttonData[selectedQuickBar].buttonText.text = overlayToWeaponAdapter.GetSkillFromIndex(bindingSkillIndex).Name;

            ResetLastButton();
        }
        else
        {
            // Update UI

            // Do something with button

            /*
            if (lastSelected == null)
                lastSelected.colors = ColorBlock.defaultColorBlock;
            buttons[selectedQuickBar].colors = selectedColorBlock;
            */
        }

        // Update Adapter
        overlayToWeaponAdapter.SelectedOnQuickbar(buttonData[selectedQuickBar].skillIndexInAdapter);
        //lastSelected = buttons[selectedQuickBar];
    }

    private void ResetLastButton()
    {
        if (lastSelected != null)
            lastSelected.colors = ColorBlock.defaultColorBlock;
        lastSelected = null;
        binding = false;
        bindingSkillIndex = -1;
    }

    private void PauseGame(bool pause)
    {
        if (pause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
