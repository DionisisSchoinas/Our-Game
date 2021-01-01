using UnityEngine;
using UnityEngine.UI;

public class ButtonData : MonoBehaviour
{
    public int quickBarIndex;
    public int skillIndexInAdapter;
    public int skillIndexInColumn;
    public Text buttonText;
    public Skill skill;

    public ButtonData(Skill skill, int quickBarIndex, int skillIndexInAdapter)
    {
        this.skill = skill;
        this.quickBarIndex = quickBarIndex;
        this.skillIndexInAdapter = skillIndexInAdapter;
    }

    public ButtonData(Skill skill, int quickBarIndex, int skillIndexInAdapter, int skillIndexInColumn) : this(skill, quickBarIndex, skillIndexInAdapter)
    {
        this.skillIndexInColumn = skillIndexInColumn;
    }

    public ButtonData(Skill skill, int quickBarIndex, int skillIndexInAdapter, Text buttonText) : this(skill, quickBarIndex, skillIndexInAdapter)
    {
        this.buttonText = buttonText;
        this.buttonText.text = skill.Name;
    }

    public ButtonData(Skill skill, int quickBarIndex, int skillIndexInAdapter, int skillIndexInColumn, Text buttonText) : this(skill, quickBarIndex, skillIndexInAdapter, skillIndexInColumn)
    {
        this.buttonText = buttonText;
        this.buttonText.text = skill.Name;
    }

    private void Awake()
    {
        if (buttonText == null)
        {
            buttonText = gameObject.GetComponent<Text>();
        }
    }

    public void NewValues(Skill skill, int skillIndexInAdapter)
    {
        this.skill = skill;
        this.skillIndexInAdapter = skillIndexInAdapter;
        this.buttonText.text = skill.Name;
    }
}