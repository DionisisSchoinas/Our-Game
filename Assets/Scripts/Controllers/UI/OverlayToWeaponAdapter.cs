using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayToWeaponAdapter : MonoBehaviour
{
    [SerializeField]
    private Wand wand;
    [SerializeField]
    private Sword sword;

    public Text spellNameDisplay;

    private int wandListLength;
    private int swordListLength;

    private PlayerMovementScript playerMovementScript;

    public bool isEnabled = false;
    

    // Start is called before the first frame update
    void Start()
    {
        List<string> spellNames = new List<string>();
        wandListLength = 0;
        swordListLength = 0;

        if (wand != null)
        {
            foreach (Spell s in wand.GetSpells())
            {
                spellNames.Add(s.Name());
            }
            wandListLength = wand.GetSpells().Count;
        }

        if (sword != null)
        {
            foreach (SwordEffect s in sword.GetSwordEffects())
            {
                spellNames.Add(s.Name());
            }
            swordListLength = sword.GetSwordEffects().Count;
        }

        playerMovementScript = GetComponent<PlayerMovementScript>();

        UIEventSystem.current.onHover += SetHover;
    }

    private void OnDestroy()
    {
        UIEventSystem.current.onHover -= SetHover;
    }

    public void ChangedSelection(int buttonPressed)
    {
        SetSelectedSpell(buttonPressed);
    }

    private void SetHover(string name, bool hovering)
    {
        if (playerMovementScript == null)
            return;

        playerMovementScript.lockMouseInputs = hovering;
    }

    private void SetSelectedSpell(int value)
    {
        if (value < wandListLength)
        {
            wand.SetSelectedSpell(value);
            spellNameDisplay.text = wand.GetSelectedSpell().Name();
        }
        else
        {
            sword.SetSelectedSwordEffect(value - wandListLength);
            spellNameDisplay.text = sword.GetSelectedEffect().Name();
        }
    }

    /*
    void DropdownValueChanged(Dropdown change)
    {
        int value = change.value;
        if (value < wandListLength)
        {
            wand.SetSelectedSpell(value);
        }
        else
        {
            sword.SetSelectedSwordEffect(value - wandListLength);
        }
    }

    public void Enable(bool state)
    {
        dropdown.interactable = state;
        isEnabled = state;
    }
    */
}
