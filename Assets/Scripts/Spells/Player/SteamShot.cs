using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamShot : Spell
{
    [SerializeField]
    private float speed = 0f;
    [SerializeField]
    private GameObject firedSteam;

    private SpellIndicatorController indicatorController;

    public override void FireSimple(Transform firePoint)
    {
        GameObject tmp = Instantiate(firedSteam, firePoint.position, firePoint.rotation) as GameObject;
        tmp.SendMessage("SetSpeed", speed);
        Destroy(tmp, 5f);
    }
    public override void SetIndicatorController(SpellIndicatorController controller)
    {
        indicatorController = controller;
    }

    public override void FireHold(bool holding, Transform firePoint)
    {
    }

    public override void WakeUp()
    {
    }

    public override ParticleSystem GetSource()
    {
        return ((GameObject)Resources.Load("Spells/Default Smoke Source", typeof(GameObject))).GetComponent<ParticleSystem>();
    }
}
