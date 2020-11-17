using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorToWandController : MonoBehaviour
{
    private Wand wand;

    void Start()
    {
        wand = GetComponentInParent<Wand>();
    }

    public void Fire()
    {
        wand.FireBasic();
    }

    public void Casting()
    {
        wand.CastingBasic(true);
    }

    public void StopCasting()
    {
        wand.CastingBasic(false);
    }
}
