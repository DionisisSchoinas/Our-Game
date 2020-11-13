using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWand : MonoBehaviour
{
    [SerializeField]
    private Transform simpleFirePoint;
    [SerializeField]
    private Transform channelingFirePoint;
    [SerializeField]
    private EnemySpell[] spells;

    public bool fireSimple;
    public bool fireChannel;

    private bool hold;

    private int selectedSpell;

    
    private void Start()
    {
        foreach (EnemySpell s in spells)
        {
            s.WakeUp();
        }
        if (spells.Length != 0) selectedSpell = 0;
        hold = false;

        InvokeRepeating(nameof(FireSpell), 0f, 2f);
    }

    public void SetSelectedSpell(int value)
    {
        selectedSpell = value;
    }

    public void Fire1()
    {
        spells[selectedSpell].FireSimple(simpleFirePoint);
    }

    public void Fire2(bool holding)
    {
        spells[selectedSpell].FireHold(!hold, channelingFirePoint);
        hold = !holding;
    }

    private void FireSpell()
    {
        if (fireSimple) Fire1();
        else Fire2(hold);
    }
}
