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

    private void Awake()
    {
        UIEventSystem.current.onHover += SetHover;
        UIEventSystem.current.onSkillPicked += SetSelectedSpell;
    }

    // Start is called before the first frame update
    void Start()
    {
        List<string> spellNames = new List<string>();
        wandListLength = -1;
        swordListLength = -1;

        int id = 0;

        if (wand != null)
        {
            wand.GetDefaultSpell().onCooldown = false;
            wand.GetDefaultSpell().cooldownPercentage = 0f;
            wand.GetDefaultSpell().uniqueOverlayToWeaponAdapterId = id;
            id++;
            foreach (Spell s in wand.GetSpells())
            {
                spellNames.Add(s.skillName);
                s.onCooldown = false;
                s.cooldownPercentage = 0f;
                s.uniqueOverlayToWeaponAdapterId = id;
                id++;
            }
            wandListLength = wand.GetSpells().Count;
        }

        if (sword != null)
        {
            sword.GetDefaultSwordEffect().onCooldown = false;
            sword.GetDefaultSwordEffect().cooldownPercentage = 0f;
            sword.GetDefaultSwordEffect().uniqueOverlayToWeaponAdapterId = id;
            id++;
            foreach (SwordEffect s in sword.GetSwordEffects())
            {
                spellNames.Add(s.skillName);
                s.onCooldown = false;
                s.cooldownPercentage = 0f;
                s.uniqueOverlayToWeaponAdapterId = id;
                id++;
            }
            swordListLength = sword.GetSwordEffects().Count;
        }

        playerMovementScript = GetComponent<PlayerMovementScript>();
    }

    private void OnDestroy()
    {
        UIEventSystem.current.onHover -= SetHover;
        UIEventSystem.current.onSkillPicked -= SetSelectedSpell;
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
        // Check if skill is on cooldown
        if (GetSkillFromIndex(skillIndexInAdapter).onCooldown)
            return;

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
            if (!sword.SetSelectedSwordEffect(skillIndexInAdapter - wandListLength - 1))
            {
                return;
            }
        }

        if (GetSkillFromIndex(skillIndexInAdapter).instaCast)
            UIEventSystem.current.SkillPickedRegister(skillIndexInAdapter, false); // If the skill will also "attack" after picking don't freeze it
        else
            UIEventSystem.current.SkillPickedRegister(skillIndexInAdapter, true);

        DisplaySkillName(skillIndexInAdapter);
    }

    // Displays the picked skill name
    private void DisplaySkillName(int indexInAdapter)
    {
        spellNameDisplay.text = GetSkillFromIndex(indexInAdapter).name;
    }

    public Skill GetSkillFromIndex(int index)
    {
        if (index == -1) // Looking for the default
        {
            if (wand != null)
                return wand.GetDefaultSpell();
            else if (sword != null)
                return sword.GetDefaultSwordEffect();
        }
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
