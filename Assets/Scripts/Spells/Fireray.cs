using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Fireray : Spell
{
    [SerializeField]
    private float damageTicksPerSecond = 5;
    [SerializeField]
    private GameObject laser;

    private GameObject tmpLaser;

    private SpellIndicatorController indicatorController;

    void Start()
    {
    }

    public override void WakeUp()
    {
    }

    public override void FireHold(bool holding, Transform firePoint)
    {
        if (holding)
        {
            tmpLaser = Instantiate(laser, firePoint);
            indicatorController.SelectLocation(firePoint, 3f, 18f);
            tmpLaser.SetActive(true);
        }
        else
        {
            Destroy(tmpLaser);
            indicatorController.DestroyIndicator();
        }
    }

    public override void FireSimple(Transform firePoint)
    {
    }

    public override ParticleSystem GetSource()
    {
        return ((GameObject)Resources.Load("Spells/Default Fire Source", typeof(GameObject))).GetComponent<ParticleSystem>();
    }
    public override void SetIndicatorController(SpellIndicatorController controller)
    {
        indicatorController = controller;
    }
}