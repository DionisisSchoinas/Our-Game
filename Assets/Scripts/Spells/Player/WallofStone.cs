using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallofStone : Spell
{
    private GameObject wallToSpawn;

    private List<GameObject> collisions;
    private int damageablesLayer;

    private Vector3 spawningLocation;
    private Vector3 spellRotation;
    private bool pickedSpot;
    private bool spawned;
    private SpellIndicatorController indicatorController;
    private float face;

    private void Start()
    {
        pickedSpot = false;
        spawned = false;
        wallToSpawn = GetComponentInChildren<WallScript>().gameObject;
    }

    public override void FireSimple(Transform firePoint)
    {
        if (pickedSpot)
        {
            indicatorController.DestroyIndicator();
            pickedSpot = false;
            spawned = true;

            Vector3 scale = new Vector3();
            if (face == 0)
            {
                scale = wallToSpawn.transform.localScale;
            }
            else if (face == 1)
            {
                scale = new Vector3(
                    wallToSpawn.transform.localScale.y,
                    wallToSpawn.transform.localScale.x,
                    wallToSpawn.transform.localScale.z
                );
            }

            GameObject w = Instantiate(wallToSpawn);
            w.SendMessage("SetScale", scale);
            w.SetActive(false);
            w.transform.eulerAngles = spellRotation;
            w.transform.position = -w.transform.up * w.transform.localScale.y / 2f + spawningLocation;
            w.SetActive(true);
        }
    }

    public override void FireHold(bool holding, Transform firePoint)
    {
        if (holding)
        {
            Vector3 scale = wallToSpawn.transform.localScale;
            scale.z = Mathf.Max(2f, wallToSpawn.transform.localScale.z);
            indicatorController.SelectLocation(20f, scale);
            pickedSpot = false;
        }
        else
        {
            if (indicatorController != null)
            {
                spawningLocation = indicatorController.LockLocation()[0];
                spellRotation = indicatorController.LockLocation()[1];
                face = indicatorController.LockLocation()[2].x;
                pickedSpot = true;
                spawned = false;
                Invoke(nameof(CancelSpell), 5f);
            }
        }
    }

    public override void WakeUp()
    {
        Start();
    }

    private void CancelSpell()
    {
        if (!spawned)
        {
            indicatorController.DestroyIndicator();
            pickedSpot = false;
        }
    }

    public override ParticleSystem GetSource()
    {
        return ResourceManager.Default.Earth;
    }

    public override void SetIndicatorController(SpellIndicatorController controller)
    {
        indicatorController = controller;
    }
}
