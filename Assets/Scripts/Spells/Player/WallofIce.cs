using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallofIce : Spell
{
    private Vector3 spawningLocation;
    private Vector3 spellRotation;
    private bool pickedSpot;
    private SpellIndicatorController indicatorController;
    private IndicatorResponse indicatorResponse;

    private void Start()
    {
        pickedSpot = false;
    }

    public override void FireSimple(Transform firePoint)
    {
        if (pickedSpot)
        {
            indicatorController.DestroyIndicator();
            pickedSpot = false;
            GameObject wall = Instantiate(gameObject);
            wall.transform.position = spawningLocation;
            wall.transform.eulerAngles = spellRotation;
            wall.SetActive(true);
            Destroy(wall, 15f);
        }
    }

    public override void FireHold(bool holding, Transform firePoint)
    {
        if (holding)
        {
            indicatorController.SelectLocation(20f, 24f, 4f);
            pickedSpot = false;
        }
        else
        {
            if (indicatorController != null)
            {
                indicatorResponse = indicatorController.LockLocation();
                if (!indicatorResponse.isNull)
                {
                    spawningLocation = indicatorResponse.centerOfAoe;
                    spellRotation = indicatorResponse.spellRotation;
                    pickedSpot = true;
                    Invoke(nameof(CancelSpell), indicatorController.indicatorDeleteTimer);
                }
            }
        }
    }

    private void CancelSpell()
    {
        if (pickedSpot)
        {
            indicatorController.DestroyIndicator();
            pickedSpot = false;
        }
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Ice;
    }

    public override void SetIndicatorController(SpellIndicatorController controller)
    {
        indicatorController = controller;
    }

    public override void WakeUp()
    {
    }
}
