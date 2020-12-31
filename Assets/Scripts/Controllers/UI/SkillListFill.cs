using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkillListFill : MonoBehaviour
{
    public class SkillTypeContainer
    {
        public string type;
        public List<Skill> skills;
        public List<int> indexes;

        public SkillTypeContainer()
        {
            this.type = "";
            this.skills = new List<Skill>();
            this.indexes = new List<int>();
        }

        public SkillTypeContainer(string type) : this()
        {
            this.type = type;
        }

        public void Add(Skill skill, int index)
        {
            skills.Add(skill);
            indexes.Add(index);
        }
    }


    [HideInInspector]
    public OverlayToWeaponAdapter weaponAdapter;
    public GameObject columnContentHolder;

    private Skill[] skills;
    [HideInInspector]
    public List<SkillTypeContainer> skillColumns;

    private void Start()
    {
        skillColumns = new List<SkillTypeContainer>();
    }

    public void FillList()
    {
        if (weaponAdapter == null)
        {
            Debug.LogError("SkillListFill needs an OverlayToWeaponAdapter to fill the list");
            return;
        }

        skills = weaponAdapter.GetSkills();
        skillColumns = SplitSkills(skills);
        
        foreach (SkillTypeContainer column in skillColumns)
        {
            GameObject col = ResourceManager.UI.SkillListColumn;
            col = Instantiate(col, columnContentHolder.transform);

            for (int i=0; i<column.skills.Count; i++)
            {
                int index = i;
                GameObject btn = ResourceManager.UI.SkillListButton;
                btn.GetComponentInChildren<Text>().text = column.skills[index].Name;
                Instantiate(btn, col.transform)
                    .GetComponent<Button>()
                    .onClick
                    .AddListener(
                        delegate {
                            Debug.Log(index);
                            Debug.Log(column.skills[index].Name + " -> " + column.indexes[index]);
                });
            }

        }
    }

    private List<SkillTypeContainer> SplitSkills(Skill[] skills)
    {
        List<SkillTypeContainer> columns = new List<SkillTypeContainer>();
        int counter = 0;
        foreach (Skill s in skills)
        {
            if (columns.Any(cont => cont.type == s.Type)) // If skill type already registered
            {
                columns[columns.FindIndex(cont => cont.type == s.Type)].Add(s, counter);
            }
            else
            {
                columns.Add(new SkillTypeContainer(s.Type));
                columns[columns.Count - 1].Add(s, counter);
            }
            counter++;
        }
        return columns;
    }
}
