﻿using System.Collections;
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
    

    // Start is called before the first frame update
    void Start()
    {
        List<string> spellNames = new List<string>();
        wandListLength = 0;
        swordListLength = 0;

        int id = 0;

        if (wand != null)
        {
            foreach (Spell s in wand.GetSpells())
            {
                spellNames.Add(s.skillName);
                s.onCooldown = false;
                s.uniqueOverlayToWeaponAdapterId = id;
                id++;
            }
            wandListLength = wand.GetSpells().Count;
        }

        if (sword != null)
        {
            foreach (SwordEffect s in sword.GetSwordEffects())
            {
                spellNames.Add(s.skillName);
                s.onCooldown = false;
                s.uniqueOverlayToWeaponAdapterId = id;
                id++;
            }
            swordListLength = sword.GetSwordEffects().Count;
        }

        playerMovementScript = GetComponent<PlayerMovementScript>();

        UIEventSystem.current.onHover += SetHover;
        UIEventSystem.current.onSkillPicked += SetSelectedSpell;
        UIEventSystem.current.onSkillListUp += SkillListUp;
    }

    private void OnDestroy()
    {
        UIEventSystem.current.onHover -= SetHover;
        UIEventSystem.current.onSkillPicked -= SetSelectedSpell;
        UIEventSystem.current.onSkillListUp -= SkillListUp;
    }

    // Triggers by the event when user hovers over a UI Element
    private void SetHover(bool hovering)
    {
        if (playerMovementScript == null)
            return;

        playerMovementScript.lockMouseInputs = hovering;
    }

    // Based on the total index triggers appropriate spell
    private void SetSelectedSpell(int skillIndexInAdapter)
    {
        if (skillIndexInAdapter < wandListLength)
        {
            // Check if the skill coudln't be selected
            if (!wand.SetSelectedSpell(skillIndexInAdapter))
            {
                return;
            }
        }
        else
        {
            // Check if the skill coudln't be selected
            if (!sword.SetSelectedSwordEffect(skillIndexInAdapter - wandListLength))
            {
                return;
            }
        }
        UIEventSystem.current.SkillPickedRegister(skillIndexInAdapter);
        DisplaySkillName(skillIndexInAdapter);
    }

    private void SkillListUp(bool up)
    {
        if (!up)
        {

        }
    }

    // Displays the picked skill name
    private void DisplaySkillName(int indexInAdapter)
    {
        spellNameDisplay.text = GetSkillFromIndex(indexInAdapter).name;
    }

    public Skill GetSkillFromIndex(int index)
    {
        return GetSkills()[index];
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
