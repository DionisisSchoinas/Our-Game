using UnityEngine;
using UnityEngine.UI;

public class ButtonData : MonoBehaviour
{
    public int quickBarIndex;
    public int skillIndexInAdapter;
    public Text buttonText;
    public Skill skill;

    public ButtonData(Skill skill, int quickBarIndex, int skillIndexInAdapter)
    {
        this.skill = skill;
        this.quickBarIndex = quickBarIndex;
        this.skillIndexInAdapter = skillIndexInAdapter;
    }

    public ButtonData(Skill skill, int quickBarIndex, int skillIndexInAdapter, Text buttonText) : this(skill, quickBarIndex, skillIndexInAdapter)
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