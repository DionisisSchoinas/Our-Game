using UnityEngine;
using UnityEngine.UI;

public class OverlayControls : MonoBehaviour
{
    public OverlayToWeaponAdapter overlayToWeaponAdapter;

    private Button[] buttons;
    private int lastSelected;
    private ColorBlock selectedColorBlock;

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
    }

    public void SetSelected(int selected)
    {
        if (lastSelected != -1)
            buttons[lastSelected].colors = ColorBlock.defaultColorBlock;

        overlayToWeaponAdapter.ChangedSelection(selected);
        buttons[selected].colors = selectedColorBlock;
        lastSelected = selected;
    }
}
