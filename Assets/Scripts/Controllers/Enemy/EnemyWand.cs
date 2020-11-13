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
    public bool fireCharge;

    private bool hold;
    private bool swap;

    private int selectedSpell;

    
    private void Start()
    {
        foreach (EnemySpell s in spells)
        {
            s.WakeUp();
        }
        if (spells.Length != 0) selectedSpell = 0;
        hold = false;
        swap = true;

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
        spells[selectedSpell].FireHold(holding, channelingFirePoint);
        hold = holding;
    }

    private void FireSpell()
    {
        if (fireSimple) Fire1();
        else if (fireChannel) Fire2(!hold);
        else
        {
            if (swap)
            {
                Fire2(true);
            }
            else
            {
                Fire2(false);
                Fire1();
            }
            swap = !swap;
        }
    }
}
