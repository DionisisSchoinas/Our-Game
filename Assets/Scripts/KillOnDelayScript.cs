using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnDelayScript : MonoBehaviour
{
    private SpellIndicatorController controller;

    public void KillAfter(float seconds, SpellIndicatorController controller)
    {
        StartCoroutine(Kill(seconds));
        this.controller = controller;
    }

    IEnumerator Kill(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (controller != null)
        {
            controller.mode = -1;
            controller.picking = false;
        }

        if (gameObject != null)
            Destroy(gameObject);
    }
}
