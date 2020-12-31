using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                spellNames.Add(s.Name);
            }
            wandListLength = wand.GetSpells().Count;
        }

        if (sword != null)
        {
            foreach (SwordEffect s in sword.GetSwordEffects())
            {
                spellNames.Add(s.Name);
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
            spellNameDisplay.text = wand.GetSelectedSpell().Name;
        }
        else
        {
            sword.SetSelectedSwordEffect(value - wandListLength);
            spellNameDisplay.text = sword.GetSelectedEffect().Name;
        }
    }

    // Returns all the skills the Controller has
    public Skill[] GetSkills()
    {
        if (wand != null && sword != null)
            return wand.GetSpells().Union<Skill>(sword.GetSwordEffects()).ToArray<Skill>();
        else if (wand != null)
            return wand.GetSpells().ToArray<Skill>();
        else if (sword != null)
            return sword.GetSwordEffects().ToArray<Skill>();
        else
            return new Skill[0];
    }
}
