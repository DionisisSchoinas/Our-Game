using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnDelayScript : MonoBehaviour
{
    public void KillAfter(float seconds)
    {
        StartCoroutine(Kill(seconds));
    }

    IEnumerator Kill(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (gameObject != null)
            Destroy(gameObject);
    }
}
