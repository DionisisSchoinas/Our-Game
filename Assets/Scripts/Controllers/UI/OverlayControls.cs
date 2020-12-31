using UnityEngine;
using UnityEngine.UI;

public class OverlayControls : MonoBehaviour
{
    public OverlayToWeaponAdapter overlayToWeaponAdapter;
    public GameObject spellListDisplay;

    private SkillListFill skillList;
    private Button[] buttons;
    private int lastSelected;
    private ColorBlock selectedColorBlock;
    private bool paused;

    private void Start()
    {
        lastSelected = -1;
        selectedColorBlock = ColorBlock.defaultColorBlock;
        selectedColorBlock.normalColor = Color.red;
        selectedColorBlock.highlightedColor = Color.magenta;

        buttons = GetComponentsInChildren<Button>();
        foreach (Button obj in buttons)
        {
            obj.gameObject.AddComponent<ElementHover>();
        }

        skillList = GetComponent<SkillListFill>();
        skillList.weaponAdapter = overlayToWeaponAdapter;
        skillList.FillList();

        spellListDisplay.SetActive(false);
        spellListDisplay.gameObject.AddComponent<ElementHover>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            SetSelected(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            SetSelected(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            SetSelected(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            SetSelected(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
        {
            SetSelected(4);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            paused = !paused;
            UIEventSystem.current.SetHover(gameObject.name, paused);
            PauseGame(paused);
            spellListDisplay.SetActive(paused);
        }
    }

    public void SetSelected(int selected)
    {
        // Update UI
        if (lastSelected != -1)
            buttons[lastSelected].colors = ColorBlock.defaultColorBlock;
        buttons[selected].colors = selectedColorBlock;

        // Update Adapter
        overlayToWeaponAdapter.ChangedSelection(selected);

        lastSelected = selected;
    }

    private void PauseGame(bool pause)
    {
        if (pause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
