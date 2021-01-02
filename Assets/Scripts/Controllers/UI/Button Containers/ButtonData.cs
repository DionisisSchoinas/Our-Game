using UnityEngine;
using UnityEngine.UI;

public class ButtonData : MonoBehaviour
{
    public int quickBarIndex;
    public int skillIndexInAdapter;
    public int skillIndexInColumn;
    public int skillColumnIndex;
    public Text buttonText;
    public Skill skill;
    public Button container;

    public ButtonData()
    {
        this.quickBarIndex = -1;
        this.skillIndexInAdapter = -1;
        this.skillIndexInColumn = -1;
        this.buttonText = null;
        this.skill = null;
        this.container = null;
    }

    // Base for all constructors
    public ButtonData(Button container, Skill skill, int quickBarIndex, int skillIndexInAdapter, int skillColumnIndex)
    {
        this.container = container;
        this.skill = skill;
        this.quickBarIndex = quickBarIndex;
        this.skillIndexInAdapter = skillIndexInAdapter;
        this.skillColumnIndex = skillColumnIndex;
        CheckForText();
    }

    public ButtonData(Button container, Skill skill, int quickBarIndex, int skillIndexInAdapter, int skillIndexInColumn, int skillColumnIndex) : this(container, skill, quickBarIndex, skillIndexInAdapter, skillColumnIndex)
    {
        this.skillIndexInColumn = skillIndexInColumn;
    }

    public ButtonData(Button container, Skill skill, int quickBarIndex, int skillIndexInAdapter, Text buttonText) : this(container, skill, quickBarIndex, skillIndexInAdapter, -1)
    {
        this.buttonText = buttonText;
        this.buttonText.text = skill.Name;
    }

    public ButtonData(Button container, Skill skill, int quickBarIndex, int skillIndexInAdapter, int skillColumnIndex, Text buttonText) : this(container, skill, quickBarIndex, skillIndexInAdapter, skillColumnIndex)
    {
        this.buttonText = buttonText;
        this.buttonText.text = skill.Name;
    }

    public ButtonData(Button container, Skill skill, int quickBarIndex, int skillIndexInAdapter, int skillIndexInColumn, int skillColumnIndex, Text buttonText) : this(container, skill, quickBarIndex, skillIndexInAdapter, skillIndexInColumn, skillColumnIndex)
    {
        this.buttonText = buttonText;
        this.buttonText.text = skill.Name;
    }

    public void NewData(ButtonData data)
    {
        this.skill = data.skill;
        this.skillIndexInAdapter = data.skillIndexInAdapter;
        CheckForText();
        this.buttonText.text = data.skill.Name;
    }

    public void CopyData(Button newButton, ButtonData data)
    {
        this.container = newButton;
        this.quickBarIndex = data.quickBarIndex;
        this.skillIndexInAdapter = data.skillIndexInAdapter;
        this.skillIndexInColumn = data.skillIndexInColumn;
        this.skillColumnIndex = data.skillColumnIndex;
        this.skill = data.skill;
        CheckForText();
        this.buttonText.text = data.skill.Name;
    }

    private void CheckForText()
    {
        if (buttonText == null)
        {
            buttonText = container.gameObject.GetComponentInChildren<Text>();
        }
    }

    public void PrintData()
    {
        Debug.Log(quickBarIndex + " " + skillIndexInAdapter + " " + skillIndexInColumn + " " + skillColumnIndex + " " + buttonText.text + " " + skill.Name + " " + container.name);
    }
}