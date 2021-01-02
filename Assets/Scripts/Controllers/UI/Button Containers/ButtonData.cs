using UnityEngine;
using UnityEngine.UI;

public class ButtonData : MonoBehaviour
{
    public int quickBarIndex;
    public int skillIndexInAdapter;
    public int skillIndexInColumn;
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

    public ButtonData(Button container, Skill skill, int quickBarIndex, int skillIndexInAdapter)
    {
        this.container = container;
        this.skill = skill;
        this.quickBarIndex = quickBarIndex;
        this.skillIndexInAdapter = skillIndexInAdapter;
        CheckForText();
    }

    public ButtonData(Button container, Skill skill, int quickBarIndex, int skillIndexInAdapter, int skillIndexInColumn) : this(container, skill, quickBarIndex, skillIndexInAdapter)
    {
        this.skillIndexInColumn = skillIndexInColumn;
    }

    public ButtonData(Button container, Skill skill, int quickBarIndex, int skillIndexInAdapter, Text buttonText) : this(container, skill, quickBarIndex, skillIndexInAdapter)
    {
        this.buttonText = buttonText;
        this.buttonText.text = skill.Name;
    }

    public ButtonData(Button container, Skill skill, int quickBarIndex, int skillIndexInAdapter, int skillIndexInColumn, Text buttonText) : this(container, skill, quickBarIndex, skillIndexInAdapter, skillIndexInColumn)
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
        Debug.Log(quickBarIndex + " " + skillIndexInAdapter + " " + skillIndexInColumn + " " + buttonText.text + " " + skill.Name + " " + container.name);
    }
}