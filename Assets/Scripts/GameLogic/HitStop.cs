using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    bool waiting;
    public void Stop(float duration)
    {
        if (waiting)
            return;
        Time.timeScale = 0.0f;
        StartCoroutine(Wait(duration));
    }

    public IEnumerator Wait(float second)
    {
        waiting = true;
        yield return new WaitForSecondsRealtime(second);
        Time.timeScale = 1.0f;
        waiting = false;
    }
}
