using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayController : MonoBehaviour
{
    [SerializeField]
    private Dropdown dropdown;
    [SerializeField]
    private Wand wand;
    [SerializeField]
    private Sword sword;

    private int wandListLength;
    private int swordListLength;

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

        dropdown.ClearOptions();
        dropdown.AddOptions(spellNames);
        dropdown.interactable = false;
        dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dropdown);
        });
    }

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
}
